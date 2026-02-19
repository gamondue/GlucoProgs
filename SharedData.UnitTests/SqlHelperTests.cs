using System;
using gamon;
using NUnit.Framework;

namespace SharedData.UnitTests
{
    /// <summary>
    /// Unit tests for the Sql static helper class.
    /// Tests SQL value formatting and escaping for SQLite queries.
    /// </summary>
    [TestFixture]
    public class SqlHelperTests
    {
        #region SqlString Tests

        [Test]
        public void SqlString_WithNullString_ReturnsNull()
        {
            // Act
            string result = Sql.SqlString(null);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlString_WithEmptyString_ReturnsEmptyQuotedString()
        {
            // Act
            string result = Sql.SqlString("");

            // Assert
            Assert.That(result, Is.EqualTo("''"));
        }

        [Test]
        public void SqlString_WithSimpleString_ReturnsQuotedString()
        {
            // Act
            string result = Sql.SqlString("test");

            // Assert
            Assert.That(result, Is.EqualTo("'test'"));
        }

        [Test]
        public void SqlString_WithSingleQuote_EscapesSingleQuote()
        {
            // Act
            string result = Sql.SqlString("It's a test");

            // Assert
            Assert.That(result, Is.EqualTo("'It''s a test'"));
        }

        [Test]
        public void SqlString_WithMultipleSingleQuotes_EscapesAllQuotes()
        {
            // Act
            string result = Sql.SqlString("It's Mary's book");

            // Assert
            Assert.That(result, Is.EqualTo("'It''s Mary''s book'"));
        }

        [Test]
        public void SqlString_WithSpecialCharacters_PreservesCharacters()
        {
            // Act
            string result = Sql.SqlString("Test @#$%&*()");

            // Assert
            Assert.That(result, Is.EqualTo("'Test @#$%&*()'"));
        }

        #endregion

        #region SqlString with MaxLength Tests

        [Test]
        public void SqlStringMaxLength_WithNullString_ReturnsNull()
        {
            // Act
            string result = Sql.SqlString(null, 10);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlStringMaxLength_WithStringUnderLimit_ReturnsFullString()
        {
            // Act
            string result = Sql.SqlString("test", 10);

            // Assert
            Assert.That(result, Is.EqualTo("'test'"));
        }

        [Test]
        public void SqlStringMaxLength_WithStringOverLimit_TruncatesString()
        {
            // Act
            string result = Sql.SqlString("This is a very long string", 10);

            // Assert
            Assert.That(result, Is.EqualTo("'This is a '"));
        }

        [Test]
        public void SqlStringMaxLength_WithZeroMaxLength_ReturnsFullString()
        {
            // Act
            string result = Sql.SqlString("test", 0);

            // Assert
            // When MaxLength is 0 or negative, the code doesn't truncate
            Assert.That(result, Is.EqualTo("'test'"));
        }

        [Test]
        public void SqlStringMaxLength_WithNegativeMaxLength_ReturnsFullString()
        {
            // Act
            string result = Sql.SqlString("test", -1);

            // Assert
            Assert.That(result, Is.EqualTo("'test'"));
        }

        #endregion

        #region SqlStringLike Tests

        [Test]
        public void SqlStringLike_WithNullString_ReturnsNull()
        {
            // Act
            string result = Sql.SqlStringLike(null);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlStringLike_WithSimpleString_ReturnsLikePattern()
        {
            // Act
            string result = Sql.SqlStringLike("test");

            // Assert
            Assert.That(result, Is.EqualTo("LIKE '%test%'"));
        }

        [Test]
        public void SqlStringLike_WithSingleQuote_EscapesQuote()
        {
            // Act
            string result = Sql.SqlStringLike("It's");

            // Assert
            Assert.That(result, Is.EqualTo("LIKE '%It''s%'"));
        }

        [Test]
        public void SqlStringLike_WithEmptyString_ReturnsLikePattern()
        {
            // Act
            string result = Sql.SqlStringLike("");

            // Assert
            Assert.That(result, Is.EqualTo("LIKE '%%'"));
        }

        #endregion

        #region SqlBool Tests

        [Test]
        public void SqlBool_WithNull_ReturnsNull()
        {
            // Act
            string result = Sql.SqlBool(null);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlBool_WithTrue_ReturnsOne()
        {
            // Act
            string result = Sql.SqlBool(true);

            // Assert
            Assert.That(result, Is.EqualTo("1"));
        }

        [Test]
        public void SqlBool_WithFalse_ReturnsZero()
        {
            // Act
            string result = Sql.SqlBool(false);

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        #endregion

        #region SqlDouble Tests

        [Test]
        public void SqlDoubleString_WithNull_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDouble((string)null);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDoubleString_WithValidDouble_ReturnsFormattedDouble()
        {
            // Act
            string result = Sql.SqlDouble("123.45");

            // Assert
            // Note: The actual behavior depends on the current culture settings
            // If culture uses comma as decimal separator, dot is ignored
            Assert.That(result, Does.Contain("123"));
        }

        [Test]
        public void SqlDoubleString_WithCommaDecimalSeparator_ConvertsToPoint()
        {
            // Act
            string result = Sql.SqlDouble("123,45");

            // Assert
            Assert.That(result, Is.EqualTo("123.45"));
        }

        [Test]
        public void SqlDoubleString_WithInvalidString_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDouble("not a number");

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDoubleString_WithNaN_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDouble(double.NaN.ToString());

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDoubleObject_WithNull_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDouble((object)null);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDoubleObject_WithValidDouble_ReturnsFormattedDouble()
        {
            // Act
            string result = Sql.SqlDouble(123.45);

            // Assert
            Assert.That(result, Is.EqualTo("123.45"));
        }

        [Test]
        public void SqlDoubleObject_WithNaN_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDouble(double.NaN);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDoubleObject_WithPositiveInfinity_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDouble(double.PositiveInfinity);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDoubleObject_WithNegativeInfinity_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDouble(double.NegativeInfinity);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDoubleObject_WithZero_ReturnsZero()
        {
            // Act
            string result = Sql.SqlDouble(0.0);

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        [Test]
        public void SqlDoubleObject_WithNegativeNumber_ReturnsNegative()
        {
            // Act
            string result = Sql.SqlDouble(-123.45);

            // Assert
            Assert.That(result, Is.EqualTo("-123.45"));
        }

        #endregion

        #region SqlFloat Tests

        [Test]
        public void SqlFloatValue_WithValidFloat_ReturnsFormattedFloat()
        {
            // Act
            string result = Sql.SqlFloat(123.45f);

            // Assert
            Assert.That(result, Is.EqualTo("123.45"));
        }

        [Test]
        public void SqlFloatValue_WithNaN_ReturnsNull()
        {
            // Act
            string result = Sql.SqlFloat(float.NaN);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlFloatValue_WithPositiveInfinity_ReturnsNull()
        {
            // Act
            string result = Sql.SqlFloat(float.PositiveInfinity);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlFloatString_WithValidString_ReturnsFormattedFloat()
        {
            // Act
            string result = Sql.SqlFloat("123.45");

            // Assert
            // Note: The actual behavior depends on the current culture settings
            Assert.That(result, Does.Contain("123"));
        }

        [Test]
        public void SqlFloatString_WithInvalidString_ReturnsNull()
        {
            // Act
            string result = Sql.SqlFloat("not a number");

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        #endregion

        #region SqlInt Tests

        [Test]
        public void SqlIntString_WithNull_ReturnsNull()
        {
            // Act
            string result = Sql.SqlInt((string)null);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlIntString_WithValidInteger_ReturnsInteger()
        {
            // Act
            string result = Sql.SqlInt("123");

            // Assert
            Assert.That(result, Is.EqualTo("123"));
        }

        [Test]
        public void SqlIntString_WithInvalidString_ReturnsNull()
        {
            // Act
            string result = Sql.SqlInt("not a number");

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlIntString_WithDecimalNumber_ReturnsNull()
        {
            // Act
            string result = Sql.SqlInt("123.45");

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlIntNullable_WithNull_ReturnsNull()
        {
            // Act
            string result = Sql.SqlInt((int?)null);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlIntNullable_WithValue_ReturnsValue()
        {
            // Act
            string result = Sql.SqlInt(123);

            // Assert
            Assert.That(result, Is.EqualTo("123"));
        }

        [Test]
        public void SqlIntNullable_WithZero_ReturnsZero()
        {
            // Act
            string result = Sql.SqlInt(0);

            // Assert
            Assert.That(result, Is.EqualTo("0"));
        }

        [Test]
        public void SqlIntNullable_WithNegative_ReturnsNegative()
        {
            // Act
            string result = Sql.SqlInt(-123);

            // Assert
            Assert.That(result, Is.EqualTo("-123"));
        }

        #endregion

        #region SqlDate Tests

        [Test]
        public void SqlDateString_WithNull_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDate((string)null);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDateString_WithEmptyString_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDate("");

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDateString_WithValidDate_ReturnsFormattedDate()
        {
            // Act
            string result = Sql.SqlDate("2024-01-15 10:30:45");

            // Assert
            Assert.That(result, Is.EqualTo("datetime('2024-01-15 10:30:45')"));
        }

        [Test]
        public void SqlDateNullable_WithNull_ReturnsNull()
        {
            // Act
            string result = Sql.SqlDate((DateTime?)null);

            // Assert
            Assert.That(result, Is.EqualTo("null"));
        }

        [Test]
        public void SqlDateNullable_WithValidDateTime_ReturnsFormattedDate()
        {
            // Arrange
            var date = new DateTime(2024, 1, 15, 10, 30, 45);

            // Act
            string result = Sql.SqlDate(date);

            // Assert
            Assert.That(result, Is.EqualTo("datetime('2024-01-15 10:30:45')"));
        }

        [Test]
        public void SqlDateNullable_WithMinValue_ReturnsFormattedDate()
        {
            // Arrange
            var date = DateTime.MinValue;

            // Act
            string result = Sql.SqlDate(date);

            // Assert
            Assert.That(result, Does.StartWith("datetime('"));
        }

        #endregion

        #region CleanStringForQuery Tests

        [Test]
        public void CleanStringForQuery_WithTabs_RemovesTabs()
        {
            // Act
            string result = Sql.CleanStringForQuery("SELECT\t*\tFROM\ttable");

            // Assert
            Assert.That(result, Is.EqualTo("SELECT * FROM table"));
        }

        [Test]
        public void CleanStringForQuery_WithNewlines_RemovesNewlines()
        {
            // Act
            string result = Sql.CleanStringForQuery("SELECT *\r\nFROM table");

            // Assert
            Assert.That(result, Is.EqualTo("SELECT * FROM table"));
        }

        [Test]
        public void CleanStringForQuery_WithMultipleSpaces_ReducesToSingleSpace()
        {
            // Act
            string result = Sql.CleanStringForQuery("SELECT  *    FROM     table");

            // Assert
            Assert.That(result, Is.EqualTo("SELECT * FROM table"));
        }

        [Test]
        public void CleanStringForQuery_WithMixedWhitespace_CleansAll()
        {
            // Act
            string result = Sql.CleanStringForQuery("SELECT\t\t*\r\n  FROM    table");

            // Assert
            Assert.That(result, Is.EqualTo("SELECT * FROM table"));
        }

        [Test]
        public void CleanStringForQuery_WithCleanString_ReturnsUnchanged()
        {
            // Act
            string result = Sql.CleanStringForQuery("SELECT * FROM table");

            // Assert
            Assert.That(result, Is.EqualTo("SELECT * FROM table"));
        }

        #endregion
    }
}
