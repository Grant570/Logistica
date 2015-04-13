using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logistica_Review;
using Logistica_Review.Controllers;
using Logistica_Review.Models;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;
using Microsoft.AspNet.Identity;
using Logistica_Review.Database;



//https://www.visualstudio.com/get-started/code/create-and-run-unit-tests-vs
namespace Logistica_Review_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestHomeController()
        {
            var controller = new HomeController();
            var result = controller.Index() as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        [TestMethod]
        public void TestAccountController()
        {
            var controller = new AccountController();
            var result = controller.Register() as ViewResult;
            Assert.AreEqual("Register", result.ViewName);
        }

        [TestMethod]
        public void TestManagerController()
        {
            var controller = new ManageController();
            var result = controller.AddPhoneNumber() as ViewResult;
            Assert.AreEqual("AddPhoneNumber", result.ViewName);
        }

        [TestMethod]
        public void TestRegistration()
        {
            var controller = new AccountController();

            var result = controller.Register(new RegisterViewModel
            {
                FirstName = "TestFirst",
                LastName = "TestLast",
                Email = "test@test.com",
                Password = "PasswordTest",
                ConfirmPassword = "PasswordTest"
            }
                ).Result;

            Assert.AreEqual(result, "");

        }

        [TestMethod]
        public void TestLogin()
        {
            var controller = new AccountController();
            var result = controller.Login(null) as ViewResult;
            Assert.AreEqual("Login", result.ViewName);
        }

    }
}
