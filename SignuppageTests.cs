using Microsoft.VisualStudio.TestTools.UnitTesting;
using Auradent.pages;
using System.Windows;

namespace Auradent.Tests
{
    [TestClass]
    public class SignuppageTests
    {
        private Signuppage _signuppage;

        [TestInitialize]
        public void Setup()
        {
            _signuppage = new Signuppage();
        }

        [TestMethod]
        public void Textboxsignup_Loaded_ShouldInitializeCorrectly()
        {
            // Arrange
            var sender = new object();
            var e = new RoutedEventArgs();

            // Act
            _signuppage.Textboxsignup_Loaded(sender, e);

            // Assert
            // Add assertions to verify the expected behavior
        }

        [TestMethod]
        public void Textboxsignup_Loaded_1_ShouldInitializeCorrectly()
        {
            // Arrange
            var sender = new object();
            var e = new RoutedEventArgs();

            // Act
            _signuppage.Textboxsignup_Loaded_1(sender, e);

            // Assert
            // Add assertions to verify the expected behavior
        }

        [TestMethod]
        public void Signup_btn_Click_ShouldNavigateToSignuppage()
        {
            // Arrange
            var sender = new object();
            var e = new RoutedEventArgs();

            // Act
            _signuppage.Signup_btn_Click(sender, e);

            // Assert
            // Add assertions to verify the expected behavior
        }
    }
}
