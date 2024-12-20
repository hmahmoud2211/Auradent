using Auradent;
using Auradent.View.Usercontrols;
using System.Windows;
using Xunit;

namespace Auradent.Tests
{
    public class MainWindowTests
    {
        [Fact]
        public void Textboxsignup_Loaded_2_ShouldInitializeCorrectly()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var textbox = new Textboxsignup();

            // Act
            mainWindow.Textboxsignup_Loaded_2(textbox, null);

            // Assert
            // Add assertions to verify the expected behavior
        }

        [Fact]
        public void Signup_btn_Click_ShouldHandleClickEvent()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var button = new Button();

            // Act
            mainWindow.Signup_btn_Click(button, null);

            // Assert
            // Add assertions to verify the expected behavior
        }

        [Fact]
        public void Login_page_Click_ShouldHandleClickEvent()
        {
            // Arrange
            var mainWindow = new MainWindow();
            var button = new Button();

            // Act
            mainWindow.Login_page_Click(button, null);

            // Assert
            // Add assertions to verify the expected behavior
        }
    }
}
