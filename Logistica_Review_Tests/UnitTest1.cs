using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logistica_Review;
using Logistica_Review.Controllers;
using Logistica_Review.Models;
using System.Web.Mvc;
using System.Text;
using System.Collections.Generic;

//https://www.visualstudio.com/get-started/code/create-and-run-unit-tests-vs
namespace Logistica_Review_Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            HomeController controller = new HomeController();

            ViewResult result = controller.Index() as ViewResult;

            //this needs work
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
