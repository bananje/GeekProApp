using Microsoft.VisualStudio.TestTools.UnitTesting;
using GeekProProgress.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekProProgress.Classes.Tests
{
    [TestClass()]
    public class ValidatorTests
    {
        [TestMethod]
        public void ValidationPassword_EmptyStrings_ReturnsFalse()
        {
            //Arrange
            string FIO = "";
            string login = "";
            string password = "";
            bool expected = false;

            //Act
            bool result = Validator.ValidationPassword(FIO, login, password);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ValidationPassword_InvalidPasswordValidLogin_ReturnsFalse()
        {
            //Arrange
            string FIO = "Лапоух Ульяна Александровна";
            string login = "lapushochek";
            string password = "?123AC_+]";
            bool expected = false;

            //Act
            bool result = Validator.ValidationPassword(FIO, login, password);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ValidationPassword_ValidPasswordInvalidLogin_ReturnsFalse()
        {
            //Arrange
            string FIO = "Лапоух Ульяна Александровна";
            string login = "lapu";
            string password = "14052004";
            bool expected = false;

            //Act
            bool result = Validator.ValidationPassword(FIO, login, password);

            //Assert
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void ValidationPassword_ValidAllData_ReturnsFalse()
        {
            //Arrange
            string FIO = "Лапоух Ульяна Александровна";
            string login = "lapushochek";
            string password = "14052004";
            bool expected = true;

            //Act
            bool result = Validator.ValidationPassword(FIO, login, password);

            //Assert
            Assert.AreEqual(expected, result);
        }
    }
}