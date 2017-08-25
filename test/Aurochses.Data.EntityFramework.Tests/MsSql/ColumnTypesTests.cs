﻿using Aurochses.Data.EntityFramework.MsSql;
using Xunit;

namespace Aurochses.Data.EntityFramework.Tests.MsSql
{
    public class ColumnTypesTests
    {
        [Fact]
        public void NVarChar_Value_Equals()
        {
            // Arrange & Act & Assert
            Assert.Equal("nvarchar", ColumnTypes.NVarChar);
        }

        [Fact]
        public void Money_Value_Equals()
        {
            // Arrange & Act & Assert
            Assert.Equal("decimal(18,4)", ColumnTypes.Money);
        }

        [Fact]
        public void DateTime_Value_Equals()
        {
            // Arrange & Act & Assert
            Assert.Equal("datetime2", ColumnTypes.DateTime);
        }

        [Fact]
        public void Date_Value_Equals()
        {
            // Arrange & Act & Assert
            Assert.Equal("date", ColumnTypes.Date);
        }

        [Fact]
        public void SpecifyNVarCharLength_Value_Equals()
        {
            // Arrange & Act & Assert
            Assert.Equal("nvarchar(255)", ColumnTypes.SpecifyNVarCharLength());
            Assert.Equal("nvarchar(100)", ColumnTypes.SpecifyNVarCharLength(100));
        }
    }
}