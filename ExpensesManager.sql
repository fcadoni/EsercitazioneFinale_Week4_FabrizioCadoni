use master
go
create database ExpensesManager
go
use ExpensesManager

create table Categories
(
	id int primary key identity(0,1),
	category varchar(100) not null 

)
select * from Categories
select * from Expenses



select * from Expenses where approved = 'false'
create table Expenses
(
	id int primary key identity(1,1),
	date datetime not null,
	categoryId int not null,
	description varchar(500),
	username varchar(100) not null,
	amount decimal(10,2),
	approved bit not null,
	constraint FK_Categories foreign key (categoryId) references Categories(id),
	constraint CHK_Amount check (amount >= 0)

)

update Categories set category = 'Generici' where id = 0

insert into Expenses values ('2022-03-08', 1, 'prova', 'user', 22.2, 'false')

insert into Categories values ('Altro')
Update Expenses set amount = 12.4
Update Expenses set approved = 'false' where id = 1