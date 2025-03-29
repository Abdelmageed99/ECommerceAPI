# E-Commerce API Documentation
**Version**: v1  
**Description**: This API provides endpoints for managing an e-commerce platform, including cart operations, order management, payment processing, product management, and user authentication.
**Base URL** : https://ecommercewebapi.runasp.net/swagger/index.html

## Authentication
All endpoints require authentication via a Bearer token (JWT). Include the token in the `Authorization` header as follows:  
`Authorization: Bearer abc123`

---

## Endpoints

### Cart Operations

#### **GET /api/Cart**
- **Tags**: Cart
- **Description**: Retrieves the current cart contents.
- **Responses**:
  - `200 Success`:
    - **Content-Type**: `application/json`, `text/json`, or `text/plain`
    - **Schema**: `CartResponse`
      ```json
      {
        "cartId": 1,              // integer (int32)
        "userId": "user123",      // string, nullable
        "totalPrice": 99.99,      // number (double)
        "items": [                // array of CartItemResponse, nullable
          {
            "cartItemId": 1,      // integer (int32)
            "productId": 123,     // integer (int32)
            "productName": "Product Name", // string, nullable
            "quantity": 2,        // integer (int32)
            "price": 49.99,       // number (double)
            "productDescription": "Description", // string, nullable
            "productCategory": "Electronics" // string, nullable
          }
        ]
      }
      ```

#### **PUT /api/Cart**
- **Tags**: Cart
- **Description**: Updates the cart with new items or quantities.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `CartRequest`
    ```json
    {
      "productId": 123,  // integer (int32), required
      "quantity": 2      // integer (int32), required
    }
    ```
- **Responses**:
  - `200 Success`: Cart updated successfully.

#### **POST /api/Cart/Add**
- **Tags**: Cart
- **Description**: Adds an item to the cart.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `CartRequest`
    ```json
    {
      "productId": 123,  // integer (int32), required
      "quantity": 2      // integer (int32), required
    }
    ```
- **Responses**:
  - `200 Success`: Item added to cart successfully.

#### **POST /api/Cart/Remove**
- **Tags**: Cart
- **Description**: Removes an item from the cart.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `CartRequest`
    ```json
    {
      "productId": 123,  // integer (int32), required
      "quantity": 1      // integer (int32), required
    }
    ```
- **Responses**:
  - `200 Success`: Item removed from cart successfully.

#### **DELETE /api/Cart/{ProductId}**
- **Tags**: Cart
- **Description**: Deletes a specific product from the cart.
- **Path Parameters**:
  - `ProductId`: integer (int32), required - The ID of the product to remove.
- **Responses**:
  - `200 Success`: Product removed from cart successfully.

#### **DELETE /api/Cart/Clear**
- **Tags**: Cart
- **Description**: Clears all items from the cart.
- **Responses**:
  - `200 Success`: Cart cleared successfully.

---

### Config Operations

#### **GET /api/Config**
- **Tags**: Config
- **Description**: Retrieves configuration settings.
- **Responses**:
  - `200 Success`: Returns configuration details (schema not specified).

---

### Orders Operations

#### **GET /api/Orders**
- **Tags**: Orders
- **Description**: Retrieves a paginated list of orders.
- **Query Parameters**:
  - `PageIndex`: integer (int32), required - The page number to retrieve.
  - `PageSize`: integer (int32), required - Number of orders per page.
  - `SearchTerm`: string, optional - Filter orders by search term.
  - `OrderBy`: string, optional - Sort orders (e.g., by date or status).
- **Responses**:
  - `200 Success`:
    - **Content-Type**: `application/json`, `text/json`, or `text/plain`
    - **Schema**: `OrdersPagedResult`
      ```json
      {
        "entities": [
          {
            "orderId": 1,              // integer (int32)
            "orderDate": "2025-03-29T12:00:00Z", // string (date-time)
            "status": "Pending",       // string, nullable
            "totalPrice": 99.99,       // number (double)
            "items": [                 // array of OrderItemResponse, nullable
              {
                "productId": 123,      // integer (int32)
                "quantity": 2,         // integer (int32)
                "price": 49.99         // number (double)
              }
            ]
          }
        ],
        "totalRecords": 50,           // integer (int32)
        "totalPages": 5,              // integer (int32)
        "currentPage": 1              // integer (int32)
      }
      ```

#### **GET /api/Orders/{OrderId}**
- **Tags**: Orders
- **Description**: Retrieves details of a specific order.
- **Path Parameters**:
  - `OrderId`: integer (int32), required - The ID of the order.
- **Responses**:
  - `200 Success`:
    - **Content-Type**: `application/json`, `text/json`, or `text/plain`
    - **Schema**: `OrderResponse`
      ```json
      {
        "orderId": 1,              // integer (int32)
        "orderDate": "2025-03-29T12:00:00Z", // string (date-time)
        "status": "Pending",       // string, nullable
        "totalPrice": 99.99,       // number (double)
        "items": [                 // array of OrderItemResponse, nullable
          {
            "productId": 123,      // integer (int32)
            "quantity": 2,         // integer (int32)
            "price": 49.99         // number (double)
          }
        ]
      }
      ```

#### **GET /api/Orders/Checkout**
- **Tags**: Orders
- **Description**: Initiates the checkout process.
- **Responses**:
  - `200 Success`: Checkout initiated (schema not specified).

#### **PUT /api/Orders/{orderId}**
- **Tags**: Orders
- **Description**: Updates the status of an order.
- **Path Parameters**:
  - `orderId`: integer (int32), required - The ID of the order to update.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `UpdateOrderStatusRequest`
    ```json
    {
      "status": "Shipped"  // string, nullable
    }
    ```
- **Responses**:
  - `200 Success`: Order status updated successfully.

#### **DELETE /api/Orders/Cancel/{OrderId}**
- **Tags**: Orders
- **Description**: Cancels a specific order.
- **Path Parameters**:
  - `OrderId`: integer (int32), required - The ID of the order to cancel.
- **Responses**:
  - `200 Success`: Order canceled successfully.

---

### Payment Operations

#### **POST /api/Payment/process**
- **Tags**: Payment
- **Description**: Processes a payment for an order.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `PaymentRequest`
    ```json
    {
      "orderId": 1,     // integer (int32), required
      "amount": 99.99   // number (double), required
    }
    ```
- **Responses**:
  - `200 Success`: Payment processed successfully.

#### **POST /api/Payment/Verify**
- **Tags**: Payment
- **Description**: Verifies a payment webhook (e.g., from Braintree).
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `BraintreeWebhookRequest`
    ```json
    {
      "transactionId": "txn_12345"  // string, nullable
    }
    ```
- **Responses**:
  - `200 Success`: Payment verified successfully.

#### **GET /api/Payment**
- **Tags**: Payment
- **Description**: Retrieves a paginated list of payments.
- **Query Parameters**:
  - `PageIndex`: integer (int32), required - The page number to retrieve.
  - `PageSize`: integer (int32), required - Number of payments per page.
  - `SearchTerm`: string, optional - Filter payments by search term.
  - `OrderBy`: string, optional - Sort payments (e.g., by date or status).
- **Responses**:
  - `200 Success`:
    - **Content-Type**: `application/json`, `text/json`, or `text/plain`
    - **Schema**: `PaymentPagedResult`
      ```json
      {
        "entities": [
          {
            "orderId": 1,            // integer (int32)
            "userId": "user123",     // string, nullable
            "paymentMethod": "CreditCard", // string, nullable
            "paymentStatus": "Completed",  // string, nullable
            "amount": 99.99,         // number (double)
            "transactionId": "txn_12345", // string, nullable
            "createdAt": "2025-03-29T12:00:00Z" // string (date-time)
          }
        ],
        "totalRecords": 50,           // integer (int32)
        "totalPages": 5,              // integer (int32)
        "currentPage": 1              // integer (int32)
      }
      ```

---

### Products Operations

#### **GET /api/Products**
- **Tags**: Products
- **Description**: Retrieves a paginated list of products.
- **Query Parameters**:
  - `PageIndex`: integer (int32), required - The page number to retrieve.
  - `PageSize`: integer (int32), required - Number of products per page.
  - `SearchTerm`: string, optional - Filter products by search term.
  - `OrderBy`: string, optional - Sort products (e.g., by price or name).
- **Responses**:
  - `200 Success`:
    - **Content-Type**: `application/json`, `text/json`, or `text/plain`
    - **Schema**: `ProductsPagedResult`
      ```json
      {
        "entities": [
          {
            "id": 123,            // integer (int32)
            "name": "Product Name", // string, nullable
            "description": "Description", // string, nullable
            "price": 49.99,       // number (double)
            "category": "Electronics" // string, nullable
          }
        ],
        "totalRecords": 100,      // integer (int32)
        "totalPages": 10,         // integer (int32)
        "currentPage": 1          // integer (int32)
      }
      ```

#### **POST /api/Products**
- **Tags**: Products
- **Description**: Creates a new product.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `ProductDTO`
    ```json
    {
      "name": "New Product",    // string, nullable
      "description": "Description", // string, nullable
      "price": 49.99,           // number (double), nullable
      "category": "Electronics" // string, nullable
    }
    ```
- **Responses**:
  - `200 Success`: Product created successfully.

#### **DELETE /api/Products**
- **Tags**: Products
- **Description**: Deletes a product.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `ProductDTO`
    ```json
    {
      "name": "Product Name",   // string, nullable
      "description": "Description", // string, nullable
      "price": 49.99,           // number (double), nullable
      "category": "Electronics" // string, nullable
    }
    ```
- **Responses**:
  - `200 Success`: Product deleted successfully.

#### **GET /api/Products/GetById/{Id}**
- **Tags**: Products
- **Description**: Retrieves a specific product by ID.
- **Path Parameters**:
  - `Id`: integer (int32), required - The ID of the product.
- **Responses**:
  - `200 Success`: Returns product details (schema not specified).

#### **PUT /api/Products/{Id}**
- **Tags**: Products
- **Description**: Updates a specific product.
- **Path Parameters**:
  - `Id`: integer (int32), required - The ID of the product to update.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `ProductDTO`
    ```json
    {
      "name": "Updated Product", // string, nullable
      "description": "New Description", // string, nullable
      "price": 59.99,           // number (double), nullable
      "category": "Electronics" // string, nullable
    }
    ```
- **Responses**:
  - `200 Success`: Product updated successfully.

---

### Users Operations

#### **POST /api/Users/SingUp**
- **Tags**: Users
- **Description**: Registers a new user.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `RegisterModel`
    ```json
    {
      "fName": "John",          // string, required
      "lName": "Doe",           // string, required
      "userName": "johndoe",    // string, required
      "fullName": "John Doe",   // string, nullable, read-only
      "email": "john@example.com", // string (email), required
      "password": "Pass123!"    // string, required
    }
    ```
- **Responses**:
  - `200 Success`:
    - **Content-Type**: `application/json`, `text/json`, or `text/plain`
    - **Schema**: `AuthModel`
      ```json
      {
        "message": "Registration successful", // string, nullable
        "isAuthenticated": true,    // boolean
        "userName": "johndoe",      // string, nullable
        "roles": ["User"],          // array of strings, nullable
        "token": "jwt_token",       // string, nullable
        "expireOn": "2025-03-30T12:00:00Z", // string (date-time)
        "refreshToken": "refresh_token", // string, nullable
        "refreshTokenExpireOn": "2025-04-29T12:00:00Z" // string (date-time)
      }
      ```

#### **POST /api/Users/SingIn**
- **Tags**: Users
- **Description**: Logs in an existing user.
- **Request Body**:
  - **Content-Type**: `application/json`, `text/json`, or `application/*+json`
  - **Schema**: `LoginModel`
    ```json
    {
      "email": "john@example.com", // string (email), required
      "password": "Pass123!"      // string, required
    }
    ```
- **Responses**:
  - `200 Success`:
    - **Content-Type**: `application/json`, `text/json`, or `text/plain`
    - **Schema**: `AuthModel`
      ```json
      {
        "message": "Login successful", // string, nullable
        "isAuthenticated": true,    // boolean
        "userName": "johndoe",      // string, nullable
        "roles": ["User"],          // array of strings, nullable
        "token": "jwt_token",       // string, nullable
        "expireOn": "2025-03-30T12:00:00Z", // string (date-time)
        "refreshToken": "refresh_token", // string, nullable
        "refreshTokenExpireOn": "2025-04-29T12:00:00Z" // string (date-time)
      }
      ```

#### **POST /api/Users/StaySingingIn**
- **Tags**: Users
- **Description**: Refreshes a user session using a refresh token.
- **Query Parameters**:
  - `refreshToken`: string, optional - The refresh token to extend the session.
- **Responses**:
  - `200 Success`:
    - **Content-Type**: `application/json`, `text/json`, or `text/plain`
    - **Schema**: `AuthModel`
      ```json
      {
        "message": "Session refreshed", // string, nullable
        "isAuthenticated": true,    // boolean
        "userName": "johndoe",      // string, nullable
        "roles": ["User"],          // array of strings, nullable
        "token": "new_jwt_token",   // string, nullable
        "expireOn": "2025-03-30T13:00:00Z", // string (date-time)
        "refreshToken": "new_refresh_token", // string, nullable
        "refreshTokenExpireOn": "2025-04-29T13:00:00Z" // string (date-time)
      }
      ```

#### **GET /api/Users**
- **Tags**: Users
- **Description**: Retrieves a paginated list of users.
- **Query Parameters**:
  - `PageIndex`: integer (int32), required - The page number to retrieve.
  - `PageSize`: integer (int32), required - Number of users per page.
  - `SearchTerm`: string, optional - Filter users by search term.
  - `OrderBy`: string, optional - Sort users (e.g., by name or email).
- **Responses**:
  - `200 Success`:
    - **Content-Type**: `application/json`, `text/json`, or `text/plain`
    - **Schema**: `PagedResultUsers`
      ```json
      {
        "entities": [
          {
            "fullName": "John Doe",   // string, nullable
            "userName": "johndoe",    // string, nullable
            "email": "john@example.com", // string, nullable
            "phoneNumber": "123-456-7890" // string, nullable
          }
        ],
        "totalRecords": 50,           // integer (int32)
        "totalPages": 5,              // integer (int32)
        "currentPage": 1              // integer (int32)
      }
      ```

---

## Schemas
The API uses the following schemas for request and response bodies. Refer to the endpoint details for usage examples.

- **`AuthModel`**: Authentication response.
- **`BraintreeWebhookRequest`**: Payment verification webhook request.
- **`CartItemResponse`**: Individual item in the cart.
- **`CartRequest`**: Cart item addition/removal request.
- **`CartResponse`**: Full cart details response.
- **`LoginModel`**: User login request.
- **`OrderItemResponse`**: Individual item in an order.
- **`OrderResponse`**: Detailed order response.
- **`OrdersPagedResult`**: Paginated list of orders.
- **`PagedResultUsers`**: Paginated list of users.
- **`PaymentPagedResult`**: Paginated list of payments.
- **`PaymentRequest`**: Payment processing request.
- **`PaymentResponse`**: Payment details.
- **`Product`**: Product details in response.
- **`ProductDTO`**: Product data for create/update/delete.
- **`ProductsPagedResult`**: Paginated list of products.
- **`RegisterModel`**: User registration request.
- **`UpdateOrderStatusRequest`**: Order status update request.
- **`UserDTO`**: User details in response.

For detailed schema structures, see the example JSON in the endpoint descriptions above.

---

This documentation reflects the exact specifications from the provided OpenAPI JSON file.
