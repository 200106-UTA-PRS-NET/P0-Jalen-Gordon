-- Create database "PizzaDb
create database PizzaDb;
go;
use PizzaDb;
go;
-- Create Schema "Revature
create schema Revature;
go;

-- Create Customer table 

create table Revature.Customer(
Username varchar(20) not null,
Passw varchar(max) not null,
Previousorder varchar(max) not null
primary key(username)
)


-- Create Store Table
create table Revature.Store(
Storename varchar(20) not null,
Storepassword varchar(20) not null
primary key(Storename)
)

--Create CustomerOrder
create table Revature.CustomerOrder(
orderId int Identity(1,1) not null,
Storename varchar(20) not null,
Username varchar(20) not null,
Price decimal(10,9),
Pizza varchar(max) not null
Primary Key(orderId)
)

-- CustomerOrder table holds foreign key (column name 'storename') from Store table
alter table Revature.CustomerOrder
add constraint FK_Store_Order_Name foreign key(storename) references Revature.Store(storename)

-- CustomerOrder table holds foreign key (column name 'username) from Customer table
alter table Revature.CustomerOrder
add constraint FK_User_Order_Name foreign key(username) references Revature.Customer(username)


--Select all data from table CustomerOrder
select * from Revature.CustomerOrder
select * from Revature.Customer
select * from Revature.Store
