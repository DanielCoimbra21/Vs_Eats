# Project Title

Welcome to VSEat. VSEat is an application dedicated to order food from some restaurants in Valais. 
By ordering food via our app, our staff will delivery it directly at your home, so you don't have to go out.


## Getting Started


### Prerequisites

To access the website, the user must be logged to the HES-SO VPN and type the following [URL](http://153.109.124.35:81/VS_Eat_Daniel_Thomas) : http://153.109.124.35:81/VS_Eat_Daniel_Thomas

To use the website, the user can create an account in SIGN-UP or you can log-in as a customer or as a staff.

To login as a staff, the user can use these following users, each one of these staff mebers has a different district where they work :

```
* daniel.coimbra@bg.ch - 123 - Martigny
* peter.berger@hotmail.com - 123 - Conthey
* kevin.cd@yahoo.fr - 123 - Entremont
* jim.dary@netplus.ch - 123 - Conches
* genevieve.dene@bluewin.ch - 123 - Hérens
* hans.merz@walliser-bote.ch - 123 - Loèche
* pierrette.mayen@hotmail.com - 123 - Brigue
* michel.beauregard@hotmail.com - 123 - Monthey
* henry.dunand@croix-rouge.ch - 123 - Rarogne
* blaise.pannatier@netplus.ch - 123 - Saint-Maurice
* yvan.salamin@netplus.ch - 123 - Sierre
* gaetan.mottet@netplus.ch - 123 - Sion
* dario.antama@netplus.ch - 123 - Viège
```


### Using the WebSite

The customer can create an account or log-in directly if he/she has an account already.
If the customer wants to create an account, the user can only use the cities that the site proposes the user.


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/loginPage.png?raw=true "Title")

![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/Sign-In.png?raw=true "Title")

Once the customer logged in, he/she will arrive at the home page. Here the user can choose the restaurant of his/her choice, or he/she can choose the distric of his/her choice.

![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/RestaurantHomePage.png?raw=true "Title")

If the customer clicks on the restaurant name, he/she will be sent to the products page of that restaurant. The customer can pass his order : 
* choose the delivery time
* quantity of each dish
* confirm order
* the orders are made for the current day only

![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/screenshotCustomer/chooseDishes.png?raw=true "Title")


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/screenshotCustomer/passYourCommand.png?raw=true "Title")


The customer will receive an email with the details of his/her order. 

If the customer clicks on the name of the district, it will show all the partner restaurants that belong to that specific district.

The customer can go to the restaurant page. There he/she can search by the restaurant name or by city name.


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/screenshotCustomer/indexRestaurant1.png?raw=true "Title")

If the customer desires to cancel his/her order he/she can do it in two differnt ways :

* Cancel order page
* * enter the order id, name and surname
* * if less than 3h before delivery it won't be possible
* Current orders page
* * click on cancel order 
* * enter name and surname 
* * if less than 3h before delivery it won't be possible


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/screenshotCustomer/cancelOrder.png?raw=true "Title")


The customer can edit the order, but only to change the quantity. But the customer can't edit it 30 minutes before the delivery time.


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/screenshotCustomer/editQuantityOrder.png?raw=true "Title")

The customer can check his/her profile by going to the parameters button. This button allows him/her to log out or check his/her profile page.

If the customer clicks on his/her profile, he/she will get to the profile page where he/she can see all of the information about his/her account.
On this page, the customer can change his/her password as well as the information about him/her.


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/screenshotCustomer/customerProfile.png?raw=true "Title")

The user can also log in as a staff. It will redirect him/her to the staff profile page. It is possible to update his/her account information as well as change his/her password.


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/loginStaff.png?raw=true "Title")


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/screenshotStaff/staffProfile.png?raw=true "Title")

The staff has 2 pages : Historic and Current Orders

* Historic : shows all the orders assigned to this specific staff
* Current Orders : Show all the orders that the staff has to deliver, he can archive it by clicking on "archive"


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/screenshotStaff/staffHistoric.png?raw=true "Title")


![Alt text](https://github.com/DanielCoimbra21/Vs_Eats/blob/dev/ImagesReadMe/screenshotStaff/staffOrders.png?raw=true "Title")

### Features
* Encrypted passwords

* Automatic emails

* Edit orders 

* Log out button

* Profile : 
* * Edit and Change password


### Project Conduct

We coded based on the MVC pattern.
First we created a MVC/MPD scheme. 
Then we uploaded the script on Visual Studio on the server annd created the DB.

Then we created a Console Solution and added it 3 layers : DTO, DAL and BLL.

Then we created a WebApp Solution and added it 3 layers : Model, View, Controller.

### Code Specifications

Login Controller : manages the login for the staff and customer, and encrypts all the passwords. It also manages the account creation.

Restaurant Controller : is used to display a list of all restaurants and take an order

Order Controller : displays a list of all orders, we can manage the orders : cancel, archive and edit

Customer Controller : manages the profile of the customer : edit, change password

Staff Controller : manages the profile of the staff : edit, change password

Mail Controller : manages all the automatic emails


## Built With

* [C# ASP.NET CORE](https://docs.microsoft.com/en-us/aspnet/core/?view=aspnetcore-6.0)
* [BOOTSTRAP](https://getbootstrap.com/docs/4.3/getting-started/introduction/) 
* [SQL WINDOWS SERVER](https://docs.microsoft.com/en-us/troubleshoot/sql/general/use-sql-server-in-windows)
* [Visual Studio]


## Authors

* **Thomas Cheseaux**
* **Carlos Daniel Coimbra Soares**

## Acknowledgments

* Nicolas Devanthery, a student of our class, by helping us with some code and giving good pieces of advice
* Xavier Barmaz by explaining us how the encryption of a password works