

using AssetTracking2_EF;
using Microsoft.EntityFrameworkCore;

System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("sv-SE");

Random roll = new Random();
User user = new User();

user.DB.Offices.RemoveRange(user.DB.Offices);
user.DB.Products.RemoveRange(user.DB.Products);
user.DB.SaveChanges();
AUTO_AddOffices(user);
AUTO_AddProducts(user, roll);

MainMenu(user);
Console.WriteLine("--program end");


static void AUTO_AddOffices(User user)
{
    Office sweden = new Office();
    sweden.Name = "Sweden";
    sweden.Currency = "SEK";

    Office denmark = new Office();
    denmark.Name = "Denmark";
    denmark.Currency = "DKK";

    Office germany = new Office();
    germany.Name = "Germany";
    germany.Currency = "EUR";

    user.Offices.Add(sweden);
    user.Offices.Add(denmark);
    user.Offices.Add(germany);
    user.DB.Offices.AddRange(user.Offices);
    user.DB.SaveChanges();
}
static void AUTO_AddProducts(User user, Random roll)
{
    string[] modelNames = new string[] { "ABC-1", "DEF-2", "GHI-3", "JKL-4", "MNO-5", "PQR-6", "STU-7", "VXY-8" };
    List<Product> temp = new List<Product>();
    foreach (string productType in user.ProductTypes)
    {
        if (productType == "Laptop")//creates total 9 laptops, one of each brand into each office
        {
            int monthCounter = -27; //this is to add date to products that are 9, 6 and 3 months away from 3 years
            foreach (Office office in user.Offices) //3 offices
            {
                int dayCounter = -1; //this is to add -1day, 0days, and +1day to that month-value, so we can see color-breakpoints in print
                foreach (string brand in user.LaptopBrands) //3 brands
                {
                    string model = modelNames[roll.Next(0, modelNames.Length - 1)];
                    double priceDollar = roll.Next(1000, 2000);
                    DateTime purchaseDate = DateTime.Today.AddMonths(monthCounter).AddDays(dayCounter);
                    dayCounter++;
                    Laptop laptop = new Laptop(productType, brand, model, priceDollar, purchaseDate, office);
                    laptop.SetPriceLocal(user, priceDollar);

                    int index = GetOfficeindex(user, office.Name);
                    user.Offices[index].Products.Add(laptop);
                    temp.Add(laptop);
                }
                monthCounter -= 3;
            }

        }
        else if (productType == "Mobile") //creates total 9 mobiles, one of each brand into each office
        {
            int monthCounter = -27; //this is to add date to products that are 9, 6 and 3 months away from 3 years
            foreach (Office office in user.Offices) //3 offices
            {
                int dayCounter = -1; //this is to add -1day, 0days, and +1day to that month-value, so we can see color-breakpoints in print
                foreach (string brand in user.MobileBrands) //3 brands
                {
                    string model = modelNames[roll.Next(0, modelNames.Length - 1)];
                    double priceDollar = roll.Next(500, 1000);
                    DateTime purchaseDate = DateTime.Today.AddMonths(monthCounter).AddDays(dayCounter);
                    dayCounter++;
                    Mobile mobile = new Mobile(productType, brand, model, priceDollar, purchaseDate, office);
                    mobile.SetPriceLocal(user, priceDollar);

                    int index = GetOfficeindex(user, office.Name);
                    user.Offices[index].Products.Add(mobile);
                    temp.Add(mobile);
                }
                monthCounter -= 3;
            }
        }
    }
    user.DB.Products.AddRange(temp);
    user.DB.SaveChanges();
}
static void MainMenu(User user)
{
    while (true)
    {
        PrintMainMenu();
        int menuChoice = VerifyIntRange(user, 1, 6); //forces int return between 1,6
        string action = "";
        string office = "";
        if (menuChoice == 6) { break; }
        else if (menuChoice == 1) { PrintMany(user, "All"); user.Stop(); continue; }
        else if (menuChoice == 2) 
        { 
            action = "View"; 
            office = ChooseOffice(user, action); //returns the name of office in a string
            PrintMany(user, office); //sends office name to the print 
            user.Stop(); 
            continue; 
        }
        else if (menuChoice == 3) { action = "Add"; }
        else if (menuChoice == 4) { action = "Delete"; }
        else if (menuChoice == 5) { action = "Edit"; }

        office = ChooseOffice(user, action); //same
        if (office == "0") { continue; }
        else if (action == "Delete") { Delete(user, office); } //to know from what office to delete from
        else if (action == "Edit") { Edit(user, office); } //or edit from
        else if (action == "Add")
        {
            string productType = ChooseProductType(user, action); //returns string: laptop or mobile
            string brand = ChooseBrand(user, action, productType); //returns chosen brand for: laptop or mobile
            Add(user, office, productType, brand); //pass it all in to add
        }
        continue;
    }
}
static string CheckIfNullOrEmpty(User user)
{ //forces user to input something, then returns it
    string input = "";
    while (true)
    {
        Console.Write("> ");
        input = Console.ReadLine().Trim();
        if (String.IsNullOrEmpty(input))
        {
            user.Error("Please input something...");
            continue;
        }
        else { return input; ; }
    }
}
static int VerifyIntRange(User user, int min, int max)
{ //forces user to input int range between min and max, and returns it
    while (true)
    {
        string input = CheckIfNullOrEmpty(user);
        bool isInt = int.TryParse(input, out int menuChoice);
        if (!isInt) { user.Error("Please input a number"); continue; }
        else if (menuChoice < min || menuChoice > max) 
        { 
            user.Error($"Please input a number within the range of {min} to {max}"); continue; 
        }
        return menuChoice;
    }
}
static double VerifyDouble(User user)
{ //forces user to input a double and returns it
    while (true)
    {
        string input = CheckIfNullOrEmpty(user);
        bool isInt = double.TryParse(input, out double number);
        if (!isInt) { user.Error("Please input a number"); continue; }
        return number;
    }
}
static DateTime VerifyDateTime(User user)
{ //forces user to input a date from the past, in specific format, then returns it
    while (true)
    {
        string input = CheckIfNullOrEmpty(user);
        bool isDate = DateTime.TryParseExact(input, "ddMMyyyy",
                                            System.Globalization.CultureInfo.InvariantCulture,
                                            System.Globalization.DateTimeStyles.None,
                                            out DateTime date);
        if (!isDate) { user.Error("Please input a date in the correct format."); continue; }
        else if (date - DateTime.Today > TimeSpan.Zero) 
        { 
            user.Error("Please input a date that has not occurred yet."); 
            continue; 
        }
        return date;
    }
}
static bool VerifyYesNo(User user, string action, string thing)
{ //forces user to enter yes or no, returns bool
    Console.WriteLine($"Are you sure you want to {action} {thing}? Y/N");
    while (true)
    {
        string YN = CheckIfNullOrEmpty(user);
        if (YN.ToLower() == "y") { return true; }
        else if (YN.ToLower() == "n") { return false; }
        else { user.Error("Please only input 'Y' for yes or 'N' for no."); continue; };
    }
}
static void PrintMainMenu()
{
    Console.Clear();
    Console.WriteLine("-MAIN MENU-");
    Console.WriteLine();
    Console.WriteLine("(1) Show all products");
    Console.WriteLine("(2) Show all products from a specific office");
    Console.WriteLine("(3) Add data");
    Console.WriteLine("(4) Delete data");
    Console.WriteLine("(5) Edit data");
    Console.WriteLine("(6) Quit app");
    Console.WriteLine();
    Console.WriteLine("To choose in the menu, please input the corresponding number");
}
static string ChooseOffice(User user, string action)
{ //prints all offices, forces user to choose, returns name of office in string.
    //action=either the choice from the main menu (CRUD), or "New" which is for the Update last stage
    Console.Clear();
    user.Offices = user.DB.Offices.ToList();

    int counter = 0;
    foreach (Office office in user.Offices)
    {
        counter++;
        Console.WriteLine($"{counter}. {office.Name} (currency: {office.Currency})");
    }
    Console.WriteLine();
    int menuChoice = 0;

    if (action == "Add")
    {
        Console.WriteLine($"Choose office to {action.ToLower()} products to, or 0 (zero) to go back");
    }
    else if (action == "New")
    {
        Console.WriteLine($"Choose new office:");
        menuChoice = VerifyIntRange(user, 0, user.Offices.Count());
        return user.Offices[menuChoice - 1].Name;
    }
    else
    {
        Console.WriteLine($"Choose office to {action.ToLower()} products from, or 0 (zero) to go back");
    }
    menuChoice = VerifyIntRange(user, 0, user.Offices.Count());
    if (menuChoice == 0) { return "0"; }
    return user.Offices[menuChoice -1].Name;
}
static string ChooseProductType(User user, string action)
{ //prints all types, forces user to choose a type, and returns the typename in string
    //action=either the choice from the main menu (CRUD), or "New" which is for the Update last stage
    Console.Clear();
    int counter = 0;
    foreach (string type in user.ProductTypes)
    {
        counter++;
        Console.WriteLine($"{counter}. {type}");
    }
    Console.WriteLine();

    if (action == "New")
    {
        Console.WriteLine("Choose new type:");
        int menuChoice = VerifyIntRange(user, 1, user.ProductTypes.Length);
        return user.ProductTypes[menuChoice - 1];
    }
    else
    {
        Console.WriteLine($"Choose what type of product you want to {action}");
        int menuChoice = VerifyIntRange(user, 1, user.ProductTypes.Length);
        return user.ProductTypes[menuChoice - 1];
    }
}
static string ChooseBrand(User user, string action, string productType)
{ //prints all brands for the specific type, forces user to choose and returns the brand in a string
    //action=either the choice from the main menu (CRUD), or "New" which is for the Update last stage
    Console.Clear();
    int menuChoice = 0;
    int counter = 0;
    if (productType == "Laptop")
    {
        foreach (string brand in user.LaptopBrands)
        {
            counter++;
            Console.WriteLine($"{counter}. {brand}");
        }
        Console.WriteLine();
        if (action == "New")
        {
            Console.WriteLine($"Choose new brand for {productType}:");
            menuChoice = VerifyIntRange(user, 1, user.LaptopBrands.Length);
            return user.LaptopBrands[menuChoice - 1];
        }
        else
        {
            Console.WriteLine($"Choose what brand of {productType} you want to {action}");
            menuChoice = VerifyIntRange(user, 1, user.LaptopBrands.Length);
            return user.LaptopBrands[menuChoice - 1];
        }
    }
    else
    {
        foreach (string brand in user.MobileBrands)
        {
            counter++;
            Console.WriteLine($"{counter}. {brand}");
        }
        Console.WriteLine(); 
        if (action == "New")
        {
            Console.WriteLine("Choose new brand:");
            menuChoice = VerifyIntRange(user, 1, user.MobileBrands.Length);
            return user.MobileBrands[menuChoice - 1];
        }
        else
        {
            Console.WriteLine($"Choose what brand of {productType} you want to {action}");
            menuChoice = VerifyIntRange(user, 1, user.MobileBrands.Length);
            return user.MobileBrands[menuChoice - 1];
        }
    }
}
static int GetOfficeindex(User user, string officeName)
{
    int index = 0;
    foreach (Office obj in user.Offices)
    {
        if (officeName == obj.Name) { break; } index++; 
    }
    return index; 
}
static string GetFullNameOfProduct(Product product)
{ // eg. ASUS-Laptop in Sweden
    string fullNameOfProduct = product.Brand + "-" +
                               product.Type + " " +
                               product.Model + " in " +
                               product.Office.Name;
    return fullNameOfProduct;
}

static void PrintMany(User user, string office)
{
    Console.Clear();
    List<Product> printList = new List<Product>();
    if (office == "All") //has this value only from main menu
    {
        printList = user.DB.Products.OrderBy(x => x.Office.Name)
                                    .ThenBy(x => x.PurchaseDate)
                                    .ToList();
    }
    else
    {
        printList = user.DB.Products.Where(x => x.Office.Name == office)
                                    .OrderBy(x => x.Type == "Laptop")
                                    .ThenBy(x => x.PurchaseDate)
                                    .ToList();
    }
    Console.WriteLine("Product type".PadRight(15) +
                      "Brand".PadRight(10) +
                      "Model".PadRight(10) +
                      "$ Price".PadRight(15) +
                      "Local".PadRight(20) +
                      "Date".PadRight(15) +
                      "Office".PadRight(10));

    for (int i = 0;i < printList.Count(); i++)
    {
        Console.Write($"{i + 1}. {printList[i].Type}".PadRight(15) +
                                 $"{printList[i].Brand}".PadRight(10) +
                                 $"{printList[i].Model}".PadRight(10) +
                                 $"{printList[i].PriceDollar.ToString("N2")}".PadRight(15) +
                                 $"{printList[i].PriceLocal.ToString("N2")} ({printList[i].Currency})".PadRight(20));
                                 user.PrintColoredDate(printList[i].PurchaseDate); //prints colored date depending on purchase date
                                 Console.Write($"{printList[i].Office.Name}".PadRight(10));
        Console.WriteLine();
    }
    Console.WriteLine();
}
static void PrintOneProduct(Product product)
{
    Console.WriteLine();
    Console.WriteLine($"1. Type of product: {product.Type}");
    Console.WriteLine($"2. Brand: {product.Brand}");
    Console.WriteLine($"3. Model: {product.Model}");
    Console.WriteLine($"4. Price in $dollar: {product.PriceDollar.ToString("N2")}");
    Console.WriteLine($"5. Local price: {product.PriceLocal.ToString("N2")}");
    Console.WriteLine($"6. Local currency: {product.Currency}");
    Console.WriteLine($"7. Purchase date (ddMMyyyy): {product.PurchaseDate.ToString("dd-MM-yyyy")}");
    Console.WriteLine($"8. In office: {product.Office.Name}");
}

static void Add(User user, string office, string productType, string brand)
{
    Console.Clear();
    Console.WriteLine($"-Add a {brand}-{productType.ToLower()} to {office}-\n"); //header
    Console.WriteLine($"What is the model of the new {brand}-{productType.ToLower()}:");
    string model = CheckIfNullOrEmpty(user);
    Console.WriteLine($"What is the price in $dollar of the new {brand}-{productType.ToLower()}:");
    double priceDollar = VerifyDouble(user);
    Console.WriteLine($"When was this {brand}-{productType.ToLower()} purchased? use format: ddMMyyyy");
    DateTime purchaseDate = VerifyDateTime(user);
    int index = GetOfficeindex(user, office);

    if (productType == "Laptop")
    {
        Laptop laptop = new Laptop(productType, brand, model, priceDollar, purchaseDate, user.Offices[index]);
        laptop.SetPriceLocal(user, priceDollar);
        string name = GetFullNameOfProduct(laptop);

        user.DB.Products.Add(laptop);
        user.DB.SaveChanges();
        user.Success($"Successfully added {name} to database.");
        user.Offices[index].Products.Add(laptop); //adds product locally too just in case
    }
    else if (productType == "Mobile")
    {
        Mobile mobile = new Mobile(productType, brand, model, priceDollar, purchaseDate, user.Offices[index]);
        mobile.SetPriceLocal(user, priceDollar);
        string name = GetFullNameOfProduct(mobile);

        user.DB.Products.Add(mobile);
        user.DB.SaveChanges();
        user.Success($"Successfully added {name} to database.");
        user.Offices[index].Products.Add(mobile); //adds product locally too just in case
    }
}
static void Delete(User user, string office)
{
    while (true)
    {
        PrintMany(user, office);
        //same list used in the print
        List<Product> printList = user.DB.Products.Where(x => x.Office.Name == office)
                                                  .OrderBy(x => x.Type == "Laptop")
                                                  .ThenBy(x => x.PurchaseDate)
                                                  .ToList();
        Console.WriteLine();
        Console.WriteLine("Choose which product to delete, or 0 (zero) to go back");
        int menuChoice = VerifyIntRange(user, 0, printList.Count());
        if (menuChoice == 0) { break; }
        Product productToDelete = printList[menuChoice - 1];
        string name = GetFullNameOfProduct(productToDelete);

        bool yes = VerifyYesNo(user, "delete", name);
        if (yes)
        {
            user.DB.Products.Remove(productToDelete);
            user.DB.SaveChanges();
            user.Success($"Successfully removed {name} from database.");

            int index = GetOfficeindex(user, office);
            user.Offices[index].Products.Remove(productToDelete); //removes product locally too just in case
            break;
        }
        else { continue; }
    }
}
static void Edit(User user, string office)
{
    while (true)
    {
        PrintMany(user, office);
        //same list used in the print
        List<Product> printList = user.DB.Products.Where(x => x.Office.Name == office)
                                                  .OrderBy(x => x.Type == "Laptop")
                                                  .ThenBy(x => x.PurchaseDate)
                                                  .ToList();
        Console.WriteLine();
        Console.WriteLine("Choose which product to edit, or 0 (zero) to go back:");
        int menuChoice = VerifyIntRange(user, 0, printList.Count());
        if (menuChoice == 0) { break; }
        Product productToEdit = printList[menuChoice - 1];
        string name = GetFullNameOfProduct(productToEdit);

        bool yes = VerifyYesNo(user, "edit", name);
        if (yes)
        {
            Console.Clear();
            PrintOneProduct(productToEdit);
            Console.WriteLine();
            Console.WriteLine("Choose what would you like to edit:");
            menuChoice = VerifyIntRange(user, 1, 9); //8 properties per product
            Console.WriteLine();
            switch (menuChoice)
            {
                case 1:
                    string newProductType = ChooseProductType(user, "New");
                    Console.WriteLine("When you change product type, you must also change brand.");
                    user.Stop();
                    string newBrand = ChooseBrand(user, "New", newProductType);
                    productToEdit.Type = newProductType;
                    productToEdit.Brand = newBrand;
                    break;
                case 2:
                    newBrand = ChooseBrand(user, "New", productToEdit.Type);
                    productToEdit.Brand = newBrand;
                    break;
                case 3:
                    Console.WriteLine("Input new model:");
                    string newModel = CheckIfNullOrEmpty(user);
                    productToEdit.Model = newModel;
                    break;
                case 4:
                    Console.WriteLine("Input new price in dollar:");
                    double newPriceDollar = VerifyDouble(user);
                    productToEdit.PriceDollar = newPriceDollar;
                    productToEdit.SetPriceLocal(user, newPriceDollar);
                    break;
                case 5:
                    Console.WriteLine($"Input new price in {productToEdit.Currency}:");
                    double newPriceLocal = VerifyDouble(user);
                    productToEdit.PriceLocal = newPriceLocal;
                    productToEdit.SetPriceDollar(user, newPriceLocal);
                    break;
                case 6:
                    Console.WriteLine("To change currency, you must change the products office.");
                    user.Stop();
                    string newOffice = ChooseOffice(user, "New");
                    int newOfficeId = user.DB.Offices.Where(x => x.Name == newOffice)
                                                     .Select(x => x.Id)
                                                     .FirstOrDefault();
                    productToEdit.Office = user.DB.Offices.Find(newOfficeId);
                    productToEdit.SetCurrency();
                    break;
                case 7:
                    Console.WriteLine("Input new date in format ddMMyyyy:");
                    DateTime newPurchaseDate = VerifyDateTime(user);
                    productToEdit.PurchaseDate = newPurchaseDate;
                    break;
                case 8:
                    newOffice = ChooseOffice(user, "New");

                    productToEdit.Office = user.DB.Offices.Where(x => x.Name == newOffice)
                                                          .Select(x => x) //selects the entire object?
                                                          .FirstOrDefault();
                    productToEdit.SetCurrency();
                    break;
            }
            user.DB.Products.Update(productToEdit);
            user.DB.SaveChanges();
            user.Success($"Successfully updated the product in the database. See new values:");

            //verify that values changed
            Product updated = user.DB.Products.Find(productToEdit.Id);
            PrintOneProduct(updated);

            int index = GetOfficeindex(user, office);
            user.Offices[index].Products.Remove(productToEdit); //removes product locally to update it
            user.Offices[index].Products.Add(updated); //adds the updated one locally too just in case

            Console.WriteLine();
            user.Stop();
            break;
        }
        else { continue; }
    }
}