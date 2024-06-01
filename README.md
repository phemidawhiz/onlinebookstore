# Online Book Store Api

Online Book Store Api for Transalliance Group

# Setup

1. Clone Repository - Git Clone https://github.com/phemidawhiz/onlinebookstore.git

2. Click the WebApi.sln to open the project in visual studio

3. Install dapper, npgsql, nunit using nuggets package manager.

4. Run the project by clicking the play button or pressing f5 key 

5. Visit the postman documentation at https://documenter.getpostman.com/view/2371999/2sA3QwbV1r for endpoint details


# High Level Design in a nut shell

OVERVIEW: The system stores and manage book information. It also collects user info and allow users to create cart/add items(Books) to cart.
Checking out involves updating the OrderStatus column of the cart to "PaidFor" and well as the PaymentOption. OrderStatus and Payment Options are enums.

SYSTEM ARCHITECTURE - https://lucid.app/documents/view/623f2b8c-be50-4047-93b8-f31d3128a1e8

