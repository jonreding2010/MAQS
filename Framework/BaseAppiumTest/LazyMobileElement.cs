﻿//--------------------------------------------------
// <copyright file="LazyMobileElement.cs" company="Magenic">
//  Copyright 2019 Magenic, All rights Reserved
// </copyright>
// <summary>This is the LazyMobileElement class</summary>
//--------------------------------------------------
using Magenic.Maqs.BaseSeleniumTest.Extensions;
using Magenic.Maqs.Utilities.Logging;
using OpenQA.Selenium;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Text;

namespace Magenic.Maqs.BaseAppiumTest
{
    /// <summary>
    /// Driver for dynamically finding and interacting with elements
    /// </summary>
    public class LazyMobileElement : IWebElement
    {
        /// <summary>
        /// A user friendly name, for logging purposes
        /// </summary>
        private readonly string userFriendlyName;

        /// <summary>
        /// The parent lazy element
        /// </summary>
        private readonly LazyMobileElement parent;

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyMobileElement" /> class
        /// </summary>
        /// <param name="testObject">The base Selenium test object</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementCreate" lang="C#" />
        /// </example>
        public LazyMobileElement(AppiumTestObject testObject, By locator, string userFriendlyName = "LazyMobileElement")
        {
            this.TestObject = testObject;
            this.By = locator;
            this.userFriendlyName = userFriendlyName;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LazyMobileElement" /> class
        /// </summary>
        /// <param name="parent">The parent lazy element</param>
        /// <param name="locator">The 'by' selector for the element</param>
        /// <param name="userFriendlyName">A user friendly name, for logging purposes</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementCreateWithParent" lang="C#" />
        /// </example>
        public LazyMobileElement(LazyMobileElement parent, By locator, string userFriendlyName = "LazyMobileElement") : this(parent.TestObject, locator, userFriendlyName)
        {
            this.parent = parent;
        }

        /// <summary>
        /// Gets a the 'by' selector for the element
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementGetBy" lang="C#" />
        /// </example>
        public By By { get; private set; }

        /// <summary>
        /// Gets the test object for the element
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementGetTestObject" lang="C#" />
        /// </example>
        public AppiumTestObject TestObject { get; private set; }

        /// <summary>
        /// Gets a cached copy of the element or null if we haven't already found the element
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyCaching" lang="C#" />
        /// </example>
        public IWebElement CachedElement { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the lazy element is enabled
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementEnabled" lang="C#" />
        /// </example>
        public bool Enabled
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).Enabled;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the lazy element is selected
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementSelected" lang="C#" />
        /// </example>
        public bool Selected
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).Selected;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the lazy element is displayed
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementDisplayed" lang="C#" />
        /// </example>
        public bool Displayed
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).Displayed;
            }
        }

        /// <summary>
        /// Gets the lazy element's tag name
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementTagName" lang="C#" />
        /// </example>
        public string TagName
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).TagName;
            }
        }

        /// <summary>
        /// Gets the lazy element's text
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementText" lang="C#" />
        /// </example>
        public string Text
        {
            get
            {
                return this.GetElement(this.GetTheExistingElement).Text;
            }
        }

        /// <summary>
        /// Gets the lazy element's location
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementLocation" lang="C#" />
        /// </example>
        public Point Location
        {
            get
            {
                return this.GetElement(this.GetTheVisibleElement).Location;
            }
        }

        /// <summary>
        /// Gets the lazy element's size
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementSize" lang="C#" />
        /// </example>
        public Size Size
        {
            get
            {
                return this.GetElement(this.GetTheVisibleElement).Size;
            }
        }

        /// <summary>
        /// Click the lazy element 
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementClick" lang="C#" />
        /// </example>
        public void Click()
        {
            IWebElement element = this.GetElement(this.GetTheClickableElement);
            this.ExecuteEvent(() => element.Click(), "Click");
        }

        /// <summary>
        /// Send keys to the lazy element
        /// </summary>
        /// <param name="text">The text to send to the lazy element</param>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementSendKeys" lang="C#" />
        /// </example>
        public void SendKeys(string text)
        {
            IWebElement element = this.GetElement(this.GetTheVisibleElement);
            this.ExecuteEvent(() => element.SendKeys(text), "SendKeys");
        }

        /// <summary>
        /// Send Secret keys with no logging
        /// </summary>
        /// <param name="text">The text to send</param>
        public void SendSecretKeys(string text)
        {
            IWebElement element = this.GetElement(this.GetTheVisibleElement);
            try
            {
                this.TestObject.Log.SuspendLogging();
                this.ExecuteEvent(() => element.SendKeys(text), "SendSecretKeys");
                this.TestObject.Log.ContinueLogging();
            }
            catch (Exception e)
            {
                this.TestObject.Log.ContinueLogging();
                this.TestObject.Log.LogMessage(MessageType.ERROR, "Exception during sending secret keys: " + e.Message + Environment.NewLine + e.StackTrace);
                throw e;
            }
        }

        /// <summary>
        /// Clear the lazy element 
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementClear" lang="C#" />
        /// </example>
        public void Clear()
        {
            IWebElement element = this.GetElement(this.GetTheVisibleElement);
            this.ExecuteEvent(() => element.Clear(), "Clear");
        }

        /// <summary>
        /// Submit the lazy element 
        /// </summary>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementSubmit" lang="C#" />
        /// </example>
        public void Submit()
        {
            IWebElement element = this.GetElement(this.GetTheExistingElement);
            this.ExecuteEvent(() => element.Submit(), "Submit");
        }

        /// <summary>
        /// Gets the value for the given attribute
        /// </summary>
        /// <param name="attributeName">The given attribute name</param>
        /// <returns>The attribute value</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementGetAttribute" lang="C#" />
        /// </example>
        public string GetAttribute(string attributeName)
        {
            return this.GetElement(this.GetTheExistingElement).GetAttribute(attributeName);
        }

        /// <summary>
        /// Gets the current value of an element - Useful for get input box text
        /// </summary>
        /// <returns>The element's current value</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementSendKeys" lang="C#" />
        /// </example>
        public string GetValue()
        {
            return this.GetElement(this.GetTheVisibleElement).GetAttribute("value");
        }

        /// <summary>
        /// Gets the CSS value for the given attribute
        /// </summary>
        /// <param name="propertyName">The given attribute/property name</param>
        /// <returns>The CSS value</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementGetCssValue" lang="C#" />
        /// </example>
        public string GetCssValue(string propertyName)
        {
            return this.GetElement(this.GetTheExistingElement).GetCssValue(propertyName);
        }

        /// <summary>
        /// Wait for and get the visible web element
        /// </summary>
        /// <returns>The web visible web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementVisibleElement" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyGetVisibleTriggerFind" lang="C#" />
        /// </example>
        public IWebElement GetTheVisibleElement()
        {
            this.CachedElement = (this.parent == null) ? this.TestObject.AppiumDriver.Wait().ForVisibleElement(this.By) :
                this.parent.GetTheExistingElement().Wait().ForVisibleElement(this.By);

            return this.CachedElement;
        }

        /// <summary>
        /// Wait for and get the clickable web element
        /// </summary>
        /// <returns>The web clickable web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementClickableElement" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyGetClickableTriggerFind" lang="C#" />
        /// </example>
        public IWebElement GetTheClickableElement()
        {
            this.CachedElement = (this.parent == null) ? this.TestObject.AppiumDriver.Wait().ForClickableElement(this.By) :
                this.parent.GetTheExistingElement().Wait().ForClickableElement(this.By);

            return this.CachedElement;
        }

        /// <summary>
        /// Wait for and get the web element
        /// </summary>
        /// <returns>The web element</returns>
        /// <example>
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyElementExistingElement" lang="C#" />
        /// <code source = "../SeleniumUnitTesting/LazyElementUnitTests.cs" region="LazyGetExistTriggerFind" lang="C#" />
        /// </example>
        public IWebElement GetTheExistingElement()
        {
            this.CachedElement = (this.parent == null) ? this.TestObject.AppiumDriver.Wait().ForElementExist(this.By) :
                this.parent.GetTheExistingElement().Wait().ForElementExist(this.By);

            return this.CachedElement;
        }

        /// <summary>
        /// Gets the value of a JavaScript property of this element
        /// </summary>
        /// <param name="propertyName">The name JavaScript the JavaScript property to get the value of</param>
        /// <returns> The JavaScript property's current value. Returns a null if the value is not set or the property does not exist</returns>
        public string GetProperty(string propertyName)
        {
            return this.GetTheExistingElement().GetProperty(propertyName);
        }

        /// <summary>
        /// Finds the first OpenQA.Selenium.IWebElement using the given method.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <returns>The first matching OpenQA.Selenium.IWebElement on the current context</returns>
        public IWebElement FindElement(By by)
        {
            return this.GetTheExistingElement().FindElement(by);
        }

        /// <summary>
        /// Finds all OpenQA.Selenium.IWebElement within the current context using the given mechanism.
        /// </summary>
        /// <param name="by">The locating mechanism to use</param>
        /// <returns>All web elements matching the current criteria, or an empty list if nothing matches</returns>
        public ReadOnlyCollection<IWebElement> FindElements(By by)
        {
            return this.GetTheExistingElement().FindElements(by);
        }

        /// <summary>
        /// Get a web element
        /// </summary>
        /// <param name="getElement">The get web element function</param>
        /// <returns>The web element</returns>
        [ExcludeFromCodeCoverage]
        private IWebElement GetElement(Func<IWebElement> getElement)
        {
            // Try to use cached element
            if (this.CachedElement != null)
            {
                try
                {
#pragma warning disable S1481 // Unused local variables should be removed
                    bool visible = this.CachedElement.Displayed;
#pragma warning restore S1481 // Unused local variables should be removed
                    return this.CachedElement;
                }
                catch (Exception e)
                {
                    this.TestObject.Log.LogMessage(MessageType.VERBOSE, "Re-finding element because: " + e.Message);
                }
            }

            try
            {
                this.TestObject.Log.LogMessage(MessageType.VERBOSE, "Performing lazy driver find on: " + this.By);
                this.CachedElement = getElement();
                return this.CachedElement;
            }
            catch (Exception e)
            {
                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.AppendLine("Failed to find: " + this.userFriendlyName);
                messageBuilder.AppendLine("Locator: " + this.By);
                messageBuilder.AppendLine("Because: " + e.Message);

                throw new Exception(messageBuilder.ToString(), e);
            }
        }

        /// <summary>
        /// Execute an element action
        /// </summary>
        /// <param name="elementAction">The element action</param>
        /// <param name="caller">Name of the action, for logging purposes</param>
        [ExcludeFromCodeCoverage]
        private void ExecuteEvent(Action elementAction, string caller)
        {
            try
            {
                this.TestObject.Log.LogMessage(MessageType.VERBOSE, "Performing lazy driver action: " + caller);
                elementAction.Invoke();
            }
            catch (Exception e)
            {
                StringBuilder messageBuilder = new StringBuilder();

                messageBuilder.AppendLine("Failed to " + caller + ": " + this.userFriendlyName);
                messageBuilder.AppendLine("Locator: " + this.By);
                messageBuilder.AppendLine("Because: " + e.Message);

                throw new Exception(messageBuilder.ToString(), e);
            }
        }
    }
}