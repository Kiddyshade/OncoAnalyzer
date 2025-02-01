using OncoAnalyzer.Helpers;


public class MenuTests
{
    [Fact]
    public void GetMenuOptionsForAdmin_ReturnsAdminOptions()
    {
        // Arrange
        var role = "Admin";

        // Act
        var options = MenuHelper.GetMenuOptionsForRole(role);

        // Assert
        Assert.Contains("1. Add Patient Details", options);
        Assert.Contains("7. Import Patients from CSV", options);
        Assert.Contains("8. Add Treatment for a patient", options);
        Assert.Contains("3. View all Patients", options);
        Assert.DoesNotContain("Access denied", options);
    }

    [Fact]

    public void GetMenuOptionsForDoctor_ReturnsDoctorOptions()
    {
        // Arrange
        var role = "Doctor";

        // Act
        var options = MenuHelper.GetMenuOptionsForRole(role);

        // Assert
        Assert.DoesNotContain("1. Add Patient Details", options);
        Assert.Contains("8. Add Treatment for a patient", options);
        Assert.Contains("3. View all Patients", options);
    }

    [Fact]
    public void GetMenuOptionsForStaff_ReturnsStaffOptions()
    {
        // Arrange
        var role = "Staff";

        // Act
        var options = MenuHelper.GetMenuOptionsForRole(role);
        // Assert
        Assert.DoesNotContain("1. Add Patient Details", options);
        Assert.DoesNotContain("1. Add Treatment for patient", options);
        Assert.Contains("3. View all Patients", options);
        Assert.Contains("5. Export all Patients to CSV", options);
    }
}