using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restaurant.Domain.Entities.MessageResponses
{
    public class Messages
    {
        // Authentication:
        public const string AuthenticationInvalidEmail = "_Invalid email address_";
        public const string AuthenticationInvalidPassword = "_Invalid password_";
        public const string AuthenticationLoginError = "_A user with the provided email address and password was not found_";

        // Users:
        public const string UserNotFound = "_User not found_";
        public const string UserUpdated = "_User updated successfully_";
        public const string UserCreated = "_User created successfully_";
        public const string UserDeleted = "_User deleted successfully_";
        public const string UserPictureCleared = "_User picture cleared successfully_";
        public const string UserNameRequired = "_Name is required_";
        public const string UserNameLength = "_Name must not exceed 100 characters_";
        public const string UserImageInvalid = "_File must be maximum of 2MB in size and in a valid image format_";
        public const string UserUsernameRequired = "_Username is required_";
        public const string UserUsernameLength = "_Username must not exceed 100 characters_";
        public const string UserEmailRequired = "_Email is required_";
        public const string UserEmailLength = "_Email must not exceed 255 characters_";
        public const string UserEmailInvalid = "_Invalid email address_";
        public const string UserEmailTaken = "_Email address is already in use_";
        public const string UserPasswordRequired = "_Password is required_";
        public const string UserPasswordLength = "_Password must be between 8 and 100 characters long_";
        public const string UserRoleRequired = "_Role is required_";
        public const string UserRoleInvalid = "_Role is invalid_";
        public const string UserAdminCannotDeleteSelf = "_You cannot delete youself_";

        // Categories:
        public const string CategoryCreated = "_Category successfully created_";
        public const string CategoryUpdated = "_Category successfully updated_";
        public const string CategoryDeleted = "_Category successfully deleted_";
        public const string CategoryNotFound = "_Category not found_";
        public const string CategoryParentInvalidId = "_Invalid parent category ID_";
        public const string CategoryNameRequired = "_Category name is required_";
        public const string CategoryNameLength = "_Category name must not exceed 100 characters_";
        public const string CategoryExists = "_Category already exists_";

        // Products:
        public const string ProductCreated = "_Product successfully created_";
        public const string ProductUpdated = "_Product successfully updated_";
        public const string ProductDeleted = "_Product successfully deleted_";
        public const string ProductNotFound = "_Product not found_";
        public const string ProductNameRequired = "_Product name is required_";
        public const string ProductNameLength = "_Product name must not exceed 100 characters_";
        public const string ProductPriceRange = "_Product price must be at least 0,01_";
        public const string ProductPriceRequired = "_Product price is required_";
        public const string ProductCategoryRequired = "_Product category id is required_";
        public const string ProductCategoryInvalid = "_Product category is invalid_";

        // Orders:
        public const string OrderCreated = "_Order successfully created_";
        public const string OrderUpdated = "_Order successfully updated_";
        public const string OrderDeleted = "_Order successfully deleted_";
        public const string OrderCompleted = "_Order successfully completed_";
        public const string OrderNotFound = "_Order not found_";
        public const string OrderAlreadyCompleted = "_Order is already completed_";
        public const string OrderUnauthorized = "_You are not authorized to make changes to this order_";
        public const string OrderTableIdRequired = "_Table id is required to create an order_";
        public const string OrderProductsRequired = "_You need at least one product to create an order_";
        public const string OrderInvalidProductId = "_Order product id is invalid_";
        public const string OrderTableInvalidId = "_Order table id is invalid_";
        public const string OrderAccessDenied = "_You do not have access to this order_";
        public const string OrderTableOccupied = "_Order table is occupied_";

        // Tables:
        public const string TableCreated = "_Table successfully created_";
        public const string TableUpdated = "_Table successfully updated_";
        public const string TableDeleted = "_Table successfully deleted_";
        public const string TableNotFound = "Table not found";
        public const string TableNotActive = "Table is not active/occupied";
        public const string TableNoOrders = "Table doesn't have any orders";
        public const string TableCleared = "Table successfully cleared";
        public const string TableInvalidId = "Invalid table ID";
        public const string TableCapacity = "Table capacity must be between 2 and 12";
        public const string TablesCapacityReached = "There is no room for more tables";
    }
}
