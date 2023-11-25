using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClientApp.Utilities.Validation;

public static class Validator
{
    public static bool ValidateCredentials(string? username, string? password)
    {
        bool isInvalidUserName = string.IsNullOrWhiteSpace(username) || username.Length > 150;
        bool isInvalidPassword = string.IsNullOrWhiteSpace(password) || password.Length > 150;

        return !(isInvalidUserName || isInvalidPassword);
    }

    public static bool ValidateProductInput(string? title, string? description, double price)
    {
        bool isInvalidTitle = string.IsNullOrWhiteSpace(title) || title.Length > 100;
        bool isInvalidDescription = string.IsNullOrWhiteSpace(description);
        bool isInvalidPrice = price <= 0;

        return !(isInvalidTitle || isInvalidDescription || isInvalidPrice);
    }
}
