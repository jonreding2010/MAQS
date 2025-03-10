﻿//-----------------------------------------------------
// <copyright file="SeleniumNUnitTest.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>NUnit test the selenium framework</summary>
//-----------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest;
using Magenic.Maqs.Utilities.Helper;
using NUnit.Framework;
using System.Diagnostics.CodeAnalysis;

namespace SeleniumUnitTests
{
    /// <summary>
    /// Test the selenium framework for NUnit
    /// </summary>
    [TestFixture]
    [ExcludeFromCodeCoverage]
    public class SeleniumNUnitTest : BaseSeleniumTest
    {
        /// <summary>
        /// Make sure we can open a browser
        /// </summary>
        [Test]
        [Category(TestCategories.NUnit)]
        public void OpenBrowser()
        {
            this.WebDriver.Navigate().GoToUrl("https://www.google.com");
        }
    }
}