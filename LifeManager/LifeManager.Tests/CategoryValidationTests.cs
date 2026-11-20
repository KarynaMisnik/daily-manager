using Xunit;
using System.ComponentModel.DataAnnotations;
using LifeManager.Api.Models;

namespace LifeManager.Tests;

public class CategoryValidationTests
{

    // rejects empty category
    [Fact]
    public void EmptyCategoryName_IsInvalid()
    {
        var category = new Category { Name = "" };

        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            category, context, results, true
        );

        Assert.False(isValid);
    }

    // whitespace-only category name
    [Fact]
    public void WhitespaceCategoryName_IsInvalid()
    {
        var category = new Category { Name = "   " };

        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            category, context, results, true
        );

        Assert.False(isValid);
    }

    // valid category name
    [Fact]
    public void ValidCategoryName_IsValid()
    {
        var category = new Category { Name = "University" };

        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            category, context, results, true
        );

        Assert.True(isValid);
    }

    // exactly 20 characters
    [Fact]
    public void CategoryNameWith20Characters_IsValid()
    {
        var category = new Category
        {
            Name = new string('A', 20)
        };

        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            category, context, results, true
        );

        Assert.True(isValid);
    }

    // more than 20 characters
    [Fact]
    public void CategoryNameWith21Characters_IsInvalid()
    {
        var category = new Category
        {
            Name = new string('A', 21)
        };

        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            category, context, results, true
        );

        Assert.False(isValid);
    }

    // verify the error message
    [Fact]
    public void CategoryNameWith21Characters_ReturnsCorrectError()
    {
        var category = new Category
        {
            Name = new string('A', 21)
        };

        var context = new ValidationContext(category);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(category, context, results, true);

        Assert.Contains(results, result =>
            result.ErrorMessage == "Category name cannot exceed 20 characters.");
    }
}