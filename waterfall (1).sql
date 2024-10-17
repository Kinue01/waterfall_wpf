create table if not exists tb_role
(
	role_id smallserial primary key,
	role_name varchar(40) not null
);

create table if not exists tb_user
(
	user_id bigserial primary key,
	user_login varchar(50) not null,
	user_pass text not null,
	user_role_id int not null,
	foreign key (user_role_id) references tb_role (role_id)
);

create table if not exists tb_position
(
	position_id smallserial primary key,
	position_name varchar(50) not null
);

create table if not exists tb_employee
(
	employee_id serial primary key,
	employee_lastname varchar(40) not null,
	employee_firstname varchar(50) not null,
	employee_middlename varchar(50),
	employee_user_id bigint not null,
	employee_position_id smallint not null,
	foreign key (employee_user_id) references tb_user (user_id),
	foreign key (employee_position_id) references tb_position (position_id)
);

create table if not exists tb_country
(
	country_id smallserial primary key,
	country_name varchar(60) not null
);

create table if not exists tb_client
(
	client_id serial primary key,
	client_firstname varchar(40) not null,
	client_lastname varchar(50) not null,
	client_middlename varchar(50),
	client_email varchar(320) not null,
	client_user_id bigint not null,
	client_country_id int not null,
	foreign key (client_user_id) references tb_user (user_id),
	foreign key (client_country_id) references tb_country (country_id)
);

create table if not exists tb_ticket_type
(
	type_id smallserial primary key,
	type_name varchar(15) not null,
	type_cost int not null
);

create table if not exists tb_session
(
	session_id serial primary key,
	session_time time not null
);

create table if not exists tb_ticket
(
	ticket_id serial primary key,
	ticket_date date not null,
	ticket_checked bool not null default false,
	ticket_client_id int not null,
	ticket_session_id int not null,
	ticket_type_id smallint not null,
	foreign key (ticket_client_id) references tb_client (client_id),
	foreign key (ticket_type_id) references tb_ticket_type (type_id),
	foreign key (ticket_session_id) references tb_session (session_id)
);

create or replace function session_tickets() returns trigger
as $$
declare
	sess int;
begin
	select count(*) from tb_ticket into sess where ticket_date = new.ticket_date and ticket_session_id = new.ticket_session_id;
	if (sess >= 15) then
		raise exception 'Больше нельзя';
	end if;
	return new;
end;
$$ language plpgsql;

create trigger trg_session_tickets after insert on tb_ticket
for each row execute function session_tickets();

insert into tb_role (role_name) values
('Клиент'), ('Сотрудник');

insert into tb_user (user_login, user_pass, user_role_id) values
('rom', 'tat', 2), 
('lob', 'iva', 2), 
('cvet', 'vla', 2), 
('com', 'eva', 1);

insert into tb_position (position_name) values
('Кассир'), ('Экскурсовод'), ('Администратор');

insert into tb_employee (employee_lastname, employee_firstname, employee_middlename, employee_user_id, employee_position_id)
values 
('Романова', 'Татьяна', 'Арсентьевна', 1, 1), 
('Лобанов', 'Иван', 'Романович', 2, 2), 
('Цветков', 'Владислав', 'Михайлович', 3, 3);

insert into tb_country (country_name) values
('США'), ('Россия'), ('Франция'), ('Германия'), ('Англия');

insert into tb_ticket_type (type_name, type_cost) values
('Взрослый', 500), ('Детский', 250);

insert into tb_client (client_firstname, client_lastname, client_middlename, client_email, client_user_id, client_country_id) values
('Комарова', 'Ева', 'Михайловна', 'vourocetrovou-6762@yopmail.com', 4, 2);

insert into tb_session (session_time) values
('6:00:00'),
('6:30:00'),
('7:00:00'),
('7:30:00'),
('8:00:00'),
('8:30:00'),
('9:00:00'),
('9:30:00'),
('10:00:00'),
('10:30:00'),
('11:00:00'),
('11:30:00'),
('12:00:00'),
('12:30:00'),
('13:00:00'),
('13:30:00'),
('14:00:00'),
('14:30:00'),
('15:00:00'),
('15:30:00'),
('16:00:00'),
('16:30:00'),
('17:00:00'),
('17:30:00'),
('18:00:00'),
('18:30:00'),
('19:00:00'),
('19:30:00'),
('20:00:00'),
('20:30:00'),
('21:00:00'),
('21:30:00'),
('22:00:00'),
('22:30:00'),
('23:00:00'),
('23:30:00'),
('00:00:00'),
('00:30:00');

insert into tb_ticket (ticket_client_id, ticket_date, ticket_session_id, ticket_type_id) values
(1, '2020-04-06', 1, 1);