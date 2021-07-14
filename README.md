# ShoppingCart API
Shopping cart API based in web api 2 and asp.net MVC.

Functionalities
The shopping cart should allow:
Add items to the shopping cart
Update his shopping cart
View the items in his shopping cart
Each shopping cart item have a price, image, description, amount and zero or more discounts.
Shopping cart should be able to display all its items and total price before and after discounts.


## 1. Compilation instructions
The solution is consisting of
* Web Service
* Business Layer
* Commmon libraries: Csv parsing, Automapper.

The first time the solution is built nuget will pull all the packages mentioned in the packages.json files from the nuget
repository.

The webservice project has the following value in the web.config
```
	<add key="ShoppingCart.CsvProductsFilePath" value="D:\ShoppingCart\src\ShoppingCart.Service\products.csv" />
	<add key="ShoppingCart.CsvDiscountsFilePath" value="D:\ShoppingCart\src\ShoppingCart.Service\discounts.csv" />
```
This is the path of the CSV file that the application needs to load the products and is the only dependency of the web service.

The entry point of the application is the ShoppingCart.Service project which loads up the MVC projec with the web api service.

The project will run on port : 50628. This is configurable within the environment.

The Web Api can be tested using the following examples:
### Product service
* http://localhost:50628/api/products To get all the products
* http://localhost:50628/api/products/1001 To Get a product by Id 1001

### Shopping Cart Service
* http://localhost:50628/api/ShoppingBasket/{cartname} . To get the shopping cart details of the given cart name
* http://localhost:50628/api/ShoppingBasket/{cartname}/Add/{productId}/{quantity} . To Add a product to the shopping cart with the given name, product id and quantity
* http://localhost:50628/api/ShoppingBasket/{cartname}/Add/{productId}/{quantity}/{discountId} . To Add a product with a specific discount to the shopping cart with the given name, product id, quantity and discountId
* http://localhost:50628/api/ShoppingBasket/{cartname}/Delete/{productId}/{quantity} . To Delete a product from the shopping cart with the given name, product id and quantity. The deletion happens if the quantity >= availableProductQuantity from Shopping cart. Else, the available product quantity in cart is decreased by quantity.
* http://localhost:50628/api/ShoppingBasket/{cartname}/Checkout . To checkout a shopping cart by the name.

There will be some scenarios where, adding the same product with and without discount, the total price may seem to be miscalculated. This happens when, for a specific product, both adding methods will be used, and the final price will account for the discount added to the product.

All the service methods are available using HTTP Get verb (for web browser testing also).

## 2. Assumptions
* Products, products discounts and the shopping carts will be stored in memory in static properties. After the products and discounts are loaded for the first time from the csv's files, everything is stored in memory and all further operations are realised in the memory.
* The CSV's files are mandatory. Exceptions have been raised otherwise.
* Shopping cart identifier will be a string, for example user's name (or any other string).
* Third party library included: CSV parsing, Automapper and others.
* For testing purposes every method will be using an HTTP verb (GET, PUT, POST, DELETE) .
* The web service can be tested directly from the browser. Postman Collection included within the project.
* The stock is reduced from the datasource when the shopping cart is being checked out. In a real world scenario the stock is not reduced by adding to the shopping cart only. The stock is checked before checkout to ensure that it has sufficient quantity to execute the order.
* Product discounts should not be available from any UI (real world scenario), but they (discounts) should appear, for example, from a table where only bussines layer has access. 
* The discount will be valoric and it will be applied to each product. For example: product1, price: 10, discount: 2 => adding 2 product1 with discount to cart, will result in final price of 16.

## 3. Design
The reason I chose this design is because I wanted every layer of the solution to be replacable with a different implementation.
I approached this project using the stairway pattern thus I created different layers for each application area and an abstractions projects where all the interfaces reside.
There are the following dependencies: The web service -> business layer -> storage layer -> csv parser.
The benefit is that every layer can be unit tested separately while mocking out all of its dependencies.
In the web service layer all the dependencies are resolved using SimpleInjector IoC container.

## 4. Future work
* Considering modifying the methods to be using async
* Further exception handling in the controller responding back appropriate status codes. So far there is only OkHttpResponse (statusCode:200), with an error object for Add (both methods), Delete and Checkout methods.
* Full tests: unit tests, user story scenario test.
* Develop UI
