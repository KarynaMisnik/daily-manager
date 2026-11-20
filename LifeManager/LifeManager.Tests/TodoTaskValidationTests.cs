using System.ComponentModel.DataAnnotations;
using LifeManager.Api.Models;
using Xunit;

namespace LifeManager.Tests;

public class TaskValidationTests
{

    // Empty value
    [Fact]
    public void EmptyTitle_ShouldFailValidation()
    {
        // Arrange
        TodoTask task = new TodoTask
        {
            Title = ""
        };

        // Act
        var context = new ValidationContext(task);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            task, context, results, true);

        // Assert
        Assert.False(isValid);
    }

    // Space as name is rejeted
    [Fact]
    public void WhitespaceTitle_ShouldFailValidation()
    {
        // Arrange
        TodoTask task = new TodoTask
        {
            Title = "   "
        };

        // Act
        var context = new ValidationContext(task);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            task, context, results, true);

        // Assert
        Assert.False(isValid);
    }

    // accepts valid names
    [Fact]
    public void ValidTitle_ShouldPassValidation()
    {
        // Arrange
        TodoTask task = new TodoTask
        {
            Title = "Study C#"
        };

        // Act
        var context = new ValidationContext(task);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            task, context, results, true);

        // Assert
        Assert.True(isValid);
    }

    // boundary test: <=100 characters will be accepted, a new task created
    [Fact]
    public void TitleWith100Characters_IsValid()
    {
        var task = new TodoTask
        {
            Title = new string('A', 100)
        };

        var context = new ValidationContext(task);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            task, context, results, true
        );

        Assert.True(isValid);
    }

    // >100 characters --> rejection, a new task will not be created, throws popup window error
    [Fact]
    public void TitleWith101Characters_IsInvalid()
    {
        var task = new TodoTask
        {
            Title = new string('A', 101)
        };

        var context = new ValidationContext(task);
        var results = new List<ValidationResult>();

        bool isValid = Validator.TryValidateObject(
            task, context, results, true
        );

        Assert.False(isValid);
    }

    // verify the API model provides the correct error message
    [Fact]
    public void TitleWith101Characters_ReturnsCorrectError()
    {
        var task = new TodoTask
        {
            Title = new string('A', 101)
        };

        var context = new ValidationContext(task);
        var results = new List<ValidationResult>();

        Validator.TryValidateObject(task, context, results, true);

        Assert.Contains(results, result =>
            result.ErrorMessage == "Task title cannot exceed 100 characters.");
    }
}