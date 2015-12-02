﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using System.Threading;

namespace NakedObjects.Web.UnitTests.Selenium
{

    public abstract class CiceroTests : AWTest
    {
        [TestMethod]
        public void Action()
        {
            //Test from home menu
            CiceroUrl("home?menu1=ProductRepository");
            WaitForOutput("Products menu.");
            //First, without paramsd
            EnterCommand("Action");
            WaitForOutput("Actions: Find Product By Name; Find Product By Number; List Products By Sub Category; List Products By Sub Categories; Find By Product Line And Class; Find By Product Lines And Classes; Find Product; Find Products By Category; New Product; Random Product; Find Product By Key; Stock Report;");
            //Filtered list
            EnterCommand("act cateGory  ");
            WaitForOutput("Actions matching category: List Products By Sub Category; Find Products By Category;");
            //No match
            EnterCommand("act foo  ");
            WaitForOutput("foo does not match any actions");
            EnterCommand("act foo, bar  ");
            WaitForOutput("Wrong number of arguments provided.");
            //single match
            EnterCommand("act rand");
            WaitForOutput("Products menu. Action: Random Product");

            //Test from object context
            CiceroUrl("object?object1=AdventureWorksModel.Product-358");
            WaitForOutput("Product: HL Grip Tape.");
            EnterCommand("Action");
            WaitForOutput("Actions: Add Or Change Photo; Best Special Offer; Associate Special Offer With Product; Open Purchase Orders For Product; Create New Work Order; Work Orders;");
            EnterCommand("act ord");
            WaitForOutput("Actions matching ord: Open Purchase Orders For Product; Create New Work Order; Work Orders;");
            EnterCommand("act foo  ");
            WaitForOutput("foo does not match any actions");
            EnterCommand("act foo, bar  ");
            WaitForOutput("Wrong number of arguments provided.");
            EnterCommand("ac best");
            WaitForOutput("Product: HL Grip Tape. Action: Best Special Offer");
            //Not available in current context
            CiceroUrl("home");
            WaitForOutput("home");
            EnterCommand("ac");
            WaitForOutput("The command: action is not available in the current context");
        }
        [TestMethod]
        public void BackAndForward() //Tested together for simplicity
        {
            CiceroUrl("home");
            WaitForOutput("home");
            EnterCommand("menu cus");
            WaitForOutput("Customers menu.");
            EnterCommand("action acc");
            WaitForOutput("Customers menu. Action: Find Customer By Account Number");
            EnterCommand("back");
            WaitForOutput("Customers menu.");
            EnterCommand("Ba");
            WaitForOutput("home");
            EnterCommand("forward");
            WaitForOutput("Customers menu.");
            EnterCommand("fO  ");
            WaitForOutput("Customers menu. Action: Find Customer By Account Number");
            //Can't go forward beyond most recent
            EnterCommand("forward");
            WaitForOutput("Customers menu. Action: Find Customer By Account Number");

            //No arguments
            EnterCommand("back x");
            WaitForOutput("Wrong number of arguments provided.");
            EnterCommand("forward y");
            WaitForOutput("Wrong number of arguments provided.");
        }
        [TestMethod]
        public void Cancel()
        {
            //Menu dialog
            CiceroUrl("home?menu1=ProductRepository&dialog1=FindProductByName");
            WaitForOutput("Products menu. Action: Find Product By Name");
            EnterCommand("cancel");
            WaitForOutput("Products menu.");
            //Test on a zero param action
            CiceroUrl("home?menu1=ProductRepository&dialog1=RandomProduct");
            WaitForOutput("Products menu. Action: Random Product");
            EnterCommand("cancel");
            WaitForOutput("Products menu.");
            //Try with argument
            CiceroUrl("home?menu1=ProductRepository&dialog1=FindProductByName");
            WaitForOutput("Products menu. Action: Find Product By Name");
            EnterCommand("cancel x");
            WaitForOutput("Wrong number of arguments provided.");

            //Object dialog
            CiceroUrl("object?object1=AdventureWorksModel.Product-358&dialog1=BestSpecialOffer");
            WaitForOutput("Product: HL Grip Tape. Action: Best Special Offer");
            EnterCommand("cancel");
            WaitForOutput("Product: HL Grip Tape.");
            //Zero param
            CiceroUrl("object?object1=AdventureWorksModel.Customer-29688&dialog1=LastOrder");
            WaitForOutput("Customer: Handy Bike Services, AW00029688. Action: Last Order");
            EnterCommand("Ca");
            WaitForOutput("Customer: Handy Bike Services, AW00029688.");

            //Cancel of Edits
            CiceroUrl("object?object1=AdventureWorksModel.Product-358&edit1=true");
            WaitForOutput("Editing Product: HL Grip Tape.");
            EnterCommand("cancel");
            WaitForOutput("Product: HL Grip Tape.");

            //Invalid contexts
            CiceroUrl("home");
            WaitForOutput("home");
            EnterCommand("cancel");
            WaitForOutput("The command: cancel is not available in the current context");
            CiceroUrl("home?menu1=ProductRepository");
            WaitForOutput("Products menu.");
            EnterCommand("cancel");
            WaitForOutput("The command: cancel is not available in the current context");
            CiceroUrl("object?object1=AdventureWorksModel.Customer-29688");
            WaitForOutput("Customer: Handy Bike Services, AW00029688.");
            EnterCommand("cancel");
            WaitForOutput("The command: cancel is not available in the current context");
        }
        [TestMethod]
        public void Edit()
        {
            CiceroUrl("object?object1=AdventureWorksModel.Customer-29688");
            WaitForOutput("Customer: Handy Bike Services, AW00029688.");
            EnterCommand("edit");
            WaitForOutput("Editing Customer: Handy Bike Services, AW00029688.");
            //No arguments
            CiceroUrl("object?object1=AdventureWorksModel.Customer-29688");
            WaitForOutput("Customer: Handy Bike Services, AW00029688.");
            EnterCommand("edit x");
            WaitForOutput("Wrong number of arguments provided.");
            //Invalid contexts
            CiceroUrl("object?object1=AdventureWorksModel.Product-358&edit1=true");
            WaitForOutput("Editing Product: HL Grip Tape.");
            EnterCommand("edit");
            WaitForOutput("The command: edit is not available in the current context");
            CiceroUrl("home");
            WaitForOutput("home");
            EnterCommand("edit");
            WaitForOutput("The command: edit is not available in the current context");
        }
        [TestMethod]
        public void Help()
        {
            //Help from home
            CiceroUrl("home");
            WaitForOutput("home");
            EnterCommand("help");
            WaitForOutput("Commands available in current context: " +
                "back; clipboard; forward; gemini; help; home; menu; where;");
            //Now try an object context
            CiceroUrl("object?object1=AdventureWorksModel.Product-943");
            WaitForOutput("Product: LL Mountain Frame - Black, 40.");
            //First with no params
            EnterCommand("help");
            WaitForOutput("Commands available in current context: action; back; clipboard; copy; description; edit; forward; gemini; go; help; home; open; property; reload; where;");
            //Now with params
            EnterCommand("help me");
            WaitForOutput("menu command: From the Home context, Menu opens a named main menu. " +
                "This command normally takes one argument: the name, or partial name, " +
                "of the menu. If the partial name matches more than one menu, a list of " +
                "matches will be returned but no menu will be opened; if no argument is " +
                "provided a list of all the menus will be returned.");
            EnterCommand("help clipboard");
            WaitForOutput("clipboard command: Reminder of the object reference currently held " +
                "in the clipboard, if any. Does not take any arguments");
            EnterCommand("help menux");
            WaitForOutput("No such command: menux");
            EnterCommand("help home back");
            WaitForOutput("No such command: home back");
            EnterCommand("help home, back");
            WaitForOutput("Wrong number of arguments provided.");
        }
        [TestMethod]
        public void Home()
        {
            //Start from another context
            CiceroUrl("object?object1=AdventureWorksModel.Product-943");
            WaitForOutput("Product: LL Mountain Frame - Black, 40.");
            //Basic test of home
            EnterCommand("home");
            WaitForOutput("home");
            //Check for captialization & spaces
            CiceroUrl("object?object1=AdventureWorksModel.Product-943");
            WaitForOutput("Product: LL Mountain Frame - Black, 40.");
            EnterCommand("  HoMe  ");
            WaitForOutput("home");
            //Now with arguments
            CiceroUrl("object?object1=AdventureWorksModel.Product-943");
            WaitForOutput("Product: LL Mountain Frame - Black, 40.");
            EnterCommand("homex");
            WaitForOutput("No such command: homex");
            EnterCommand("home x");
            WaitForOutput("Wrong number of arguments provided.");
        }
        [TestMethod]
        public void Menu()
        {   //No argument
            CiceroUrl("home");
            WaitForOutput("home");
            EnterCommand("Menu");
            WaitForOutput("Menus: Customers; Orders; Products; Employees; Sales; Special Offers; Contacts; Vendors; Purchase Orders; Work Orders;");
            //Now with arguments
            EnterCommand("Menu Customers");
            WaitForOutput("Customers menu.");
            EnterCommand("Menu pro");
            WaitForOutput("Products menu.");
            EnterCommand("Menu off");
            WaitForOutput("Special Offers menu.");
            EnterCommand("Menu ord");
            WaitForOutput("Menus matching ord: Orders; Purchase Orders; Work Orders;");
            EnterCommand("Menu foo");
            WaitForOutput("foo does not match any menu");
            EnterCommand("Menu cust prod");
            WaitForOutput("cust prod does not match any menu");
            EnterCommand("Menu cus, ord");
            WaitForOutput("Wrong number of arguments provided.");
            //Invoked in invalid context
            CiceroUrl("object?object1=AdventureWorksModel.Product-943");
            WaitForOutput("Product: LL Mountain Frame - Black, 40.");
            EnterCommand("Menu");
            WaitForOutput("The command: menu is not available in the current context");
        }
        [TestMethod]
        public void OK()
        {
            //Open a zero-param action on main menu
            CiceroUrl("home?menu1=ProductRepository&dialog1=RandomProduct");
            WaitForOutput("Products menu. Action: Random Product");
            EnterCommand("ok");
            WaitForOutputStartingWith("Product: ");
            // No arguments
            CiceroUrl("home?menu1=ProductRepository&dialog1=RandomProduct");
            WaitForOutput("Products menu. Action: Random Product");
            EnterCommand("ok x");
            WaitForOutput("Wrong number of arguments provided.");

            //Object action
            CiceroUrl("object?object1=AdventureWorksModel.Customer-29688&dialog1=LastOrder");
            WaitForOutput("Customer: Handy Bike Services, AW00029688. Action: Last Order");
            EnterCommand("ok");
            WaitForOutput("SalesOrderHeader: SO69562.");

            //Invalid contexts
            CiceroUrl("home");
            WaitForOutput("home");
            EnterCommand("ok");
            WaitForOutput("The command: ok is not available in the current context");
            CiceroUrl("home?menu1=ProductRepository");
            WaitForOutput("Products menu.");
            EnterCommand("ok");
            WaitForOutput("The command: ok is not available in the current context");


        }
        [TestMethod, Ignore]
        public void Property()
        {
            //TBD
        }
        [TestMethod, Ignore]
        public void Where()
        {
            CiceroUrl("object?object1=AdventureWorksModel.Product-358");
            WaitForOutput("Product: HL Grip Tape.");
            //Do something to change the output
            EnterCommand("help"); //?? not working !!
            WaitForOutput("Commands available in current context: back; clipboard; forward; gemini; help; home; menu; where;"); 

            EnterCommand("where");
            WaitForOutput("Product: HL Grip Tape.");

            //Empty command == where
            EnterCommand("help");
            WaitForOutput("Commands available in current context: back; clipboard; forward; gemini; help; home; menu; where;");
            TypeIntoField("input", Keys.Enter);
            WaitForOutput("Product: HL Grip Tape.");

            //No arguments
            EnterCommand("where x");
            WaitForOutput("Wrong number of arguments provided.");
        }
    }
    #region browsers specific subclasses 

    //    [TestClass, Ignore]
    public class CiceroTestsIe : CiceroTests
    {
        [ClassInitialize]
        public new static void InitialiseClass(TestContext context)
        {
            FilePath(@"drivers.IEDriverServer.exe");
            AWTest.InitialiseClass(context);
        }

        [TestInitialize]
        public virtual void InitializeTest()
        {
            InitIeDriver();
            Url(BaseUrl);
        }

        [TestCleanup]
        public virtual void CleanupTest()
        {
            base.CleanUpTest();
        }
    }

    [TestClass]
    public class CiceroTestsFirefox : CiceroTests
    {
        [ClassInitialize]
        public new static void InitialiseClass(TestContext context)
        {
            AWTest.InitialiseClass(context);
        }

        [TestInitialize]
        public virtual void InitializeTest()
        {
            InitFirefoxDriver();
            Url(BaseUrl);
        }

        [TestCleanup]
        public virtual void CleanupTest()
        {
            base.CleanUpTest();
        }
    }

    // [TestClass, Ignore]
    public class CiceroTestsChrome : CiceroTests
    {
        [ClassInitialize]
        public new static void InitialiseClass(TestContext context)
        {
            FilePath(@"drivers.chromedriver.exe");
            AWTest.InitialiseClass(context);
        }

        [TestInitialize]
        public virtual void InitializeTest()
        {
            InitChromeDriver();
            Url(BaseUrl);
        }

        [TestCleanup]
        public virtual void CleanupTest()
        {
            base.CleanUpTest();
        }

        protected override void ScrollTo(IWebElement element)
        {
            string script = string.Format("window.scrollTo(0, {0})", element.Location.Y);
            ((IJavaScriptExecutor)br).ExecuteScript(script);
        }
    }

    #endregion
}