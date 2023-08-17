drop database if exists phonestore;
create database phonestore;
use phonestore;

CREATE TABLE brands(
brand_id INT AUTO_INCREMENT PRIMARY KEY,
brand_name VARCHAR(50) UNIQUE,
website VARCHAR(100) UNIQUE
)engine = InnoDB;

-- Thêm dữ liệu cho bảng Brands
INSERT INTO brands (brand_name, website)
VALUE ('Apple', 'https://www.apple.com/vn/');
INSERT INTO brands (brand_name, website)
VALUE ('Samsung', 'https://www.samsung.com/vn/');
INSERT INTO brands (brand_name, website)
VALUE ('Xiaomi', 'https://www.mi.com/vn');
INSERT INTO brands (brand_name, website)
VALUE ('Nokia', 'https://www.nokia.com/phones/vi_vn');
INSERT INTO brands (brand_name, website)
VALUE ('Oppo', 'https://www.oppo.com/vn/');
INSERT INTO brands (brand_name, website)
VALUE ('Realme', 'https://www.realme.com/vn/');
INSERT INTO brands (brand_name, website)
VALUE ('Lenovo', 'https://www.lenovo.com/vn/vn/');

CREATE TABLE colors(
color_id INT AUTO_INCREMENT PRIMARY KEY,
color_name VARCHAR(50) UNIQUE NOT NULL
)engine = InnoDB;

-- thêm dữ liệu cho bảng colors
INSERT INTO colors(color_name)
VALUE ('Pink');
INSERT INTO colors(color_name)
VALUE ('Red');
INSERT INTO colors(color_name)
VALUE ('Yellow');
INSERT INTO colors(color_name)
VALUE ('Purple');
INSERT INTO colors(color_name)
VALUE ('Black');
INSERT INTO colors(color_name)
VALUE ('White');

CREATE TABLE romsizes(
rom_size_id INT AUTO_INCREMENT PRIMARY KEY,
rom_size VARCHAR(50) UNIQUE NOT NULL
)engine = InnoDB;
-- thêm dữ liệu bảng romsizes
INSERT INTO romsizes (rom_size)
VALUE ('16GB');
INSERT INTO romsizes (rom_size)
VALUE ('32GB');
INSERT INTO romsizes (rom_size)
VALUE ('64GB');
INSERT INTO romsizes (rom_size)
VALUE ('128GB');
INSERT INTO romsizes (rom_size)
VALUE ('256GB');
INSERT INTO romsizes (rom_size)
VALUE ('512GB');
INSERT INTO romsizes (rom_size)
VALUE ('1TB');

CREATE TABLE staffs(
staff_id INT AUTO_INCREMENT PRIMARY KEY,
name VARCHAR(50) NOT NULL,
phone_number VARCHAR(12) NOT NULL,
address VARCHAR(50),
user_name VARCHAR(20) UNIQUE NOT NULL,
password VARCHAR(100) NOT NULL,
role INT NOT NULL,
status INT DEFAULT '0'
)engine = InnoDB;
-- thêm dữ liệu staffs
INSERT INTO staffs(name, phone_number, address, user_name, password, role, status)
VALUE ('Nguyen Thien An', '0766668602','Hanoi','manager','e99a18c428cb38d5f260853678922e03',2, 1);
INSERT INTO staffs(name, phone_number, address, user_name, password, role, status)
VALUE ('Bien Tien Dat', '0789456123','Hanoi','accountant','e99a18c428cb38d5f260853678922e03',1, 1);
INSERT INTO staffs(name, phone_number, address, user_name, password, role, status)
VALUE ('Nguyen Van Accountant', '0789456123','Hanoi','accountant02','e99a18c428cb38d5f260853678922e03',1, 1);
INSERT INTO staffs(name, phone_number, address, user_name, password, role, status)
VALUE ('Tran Tien Anh', '0902126092','Hanoi','seller01','e99a18c428cb38d5f260853678922e03',0, 1);

CREATE TABLE customers(
customer_id INT AUTO_INCREMENT PRIMARY KEY,
name VARCHAR(50) NOT NULL,
phone_number VARCHAR(15) NOT NULL,
address VARCHAR(50)
)engine = InnoDB;
INSERT INTO customers(name, phone_number, address) VALUE('Yua Mikami', '0254136578', 'Quat Lam');
INSERT INTO customers(name, phone_number, address) VALUE('Erichio Masharo', '0254136548', 'Quat Lam');


delimiter $$
create procedure sp_createCustomer(
IN customerPhone VARCHAR(15), 
OUT customerID int)
begin
DECLARE phoneCount INT;
SELECT COUNT(*) INTO phoneCount FROM Customers WHERE phone_number = customerPhone;
    IF phoneCount > 0 THEN
        SELECT customer_id FROM Customers WHERE phone_number = customerPhone;
    END IF;
end $$
delimiter ;

CREATE TABLE phones(
phone_id INT PRIMARY KEY,
phone_name VARCHAR(50) UNIQUE,
brand_id INT NOT NULL,
camera VARCHAR(500) DEFAULT 'not have',
ram VARCHAR(100) DEFAULT 'not have',
weight VARCHAR(100) NOT NULL,
processor VARCHAR(500) DEFAULT 'not have',
battery_capacity VARCHAR(100) NOT NULL,
sim_slot VARCHAR(100) NOT NULL,
os VARCHAR(500) DEFAULT 'not have',
screen VARCHAR(200) NOT NULL,
charge_port VARCHAR(200) DEFAULT 'not have',
release_date DATETIME NOT NULL,
connection VARCHAR(300) DEFAULT 'not have',
description VARCHAR(1000) DEFAULT 'not have',
create_at DATETIME DEFAULT CURRENT_TIMESTAMP(),
create_by INT NOT NULL,
FOREIGN KEY(brand_id) REFERENCES brands(brand_id),
FOREIGN KEY(create_by) REFERENCES staffs(staff_id)
)engine = InnoDB;

INSERT INTO phones(phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by) 
VALUE('0','default', '2', 'default', 'default', '1','default', '1998-12-12', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('1', 'Iphone 11', '1', '194 grams', '3,110 mAh', '2', '828 x 1792','2019-09-20', '1', 'iOS', '4G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('2', 'Iphone 12', '1', '194 grams', '3,110 mAh', '2', '1828 x 1792','2019-09-20', '1', 'iOS', '4G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('3', 'Iphone 13', '1', '194 grams', '3,110 mAh', '2', '2828 x 1792','2029-09-20', '1', 'iOS', '4G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('4', 'Iphone 14', '1', '194 grams', '3,110 mAh', '2', '3828 x 1792','2039-09-20', '1', 'iOS', '4G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('5', 'Iphone 15', '1', '194 grams', '3,110 mAh', '2', '4828 x 1792','2049-09-20', '1', 'iOS', '5G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('6','Iphone 16', '1', '194 grams', '3,110 mAh', '2', '5828 x 1792','2059-09-20', '1', 'iOS', '5G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('7', 'Iphone 17', '1', '194 grams', '3,110 mAh', '2', '6828 x 1792','2069-09-20', '1', 'iOS', '6G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('8', 'Iphone 18', '1', '194 grams', '3,110 mAh', '2', '7828 x 1792','2079-09-20', '1', 'iOS', '6G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('9','Iphone 19', '1', '194 grams', '3,110 mAh', '2', '8828 x 1792','2089-09-20', '1', 'iOS', '7G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('10','Iphone XX', '1', '194 grams', '3,110 mAh', '2', '9828 x 1792','2099-09-20', '1', 'iOS', '70G/LTE');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('11', 'Samsung Note 10', '2', '200 grams', '4,000 mAh', '2', '1828 x 1792','2018-10-15', '1', 'Android','4G' );
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('12','Samsung Note 11', '2', '200 grams', '4,000 mAh', '2', '2828 x 1792','2019-10-15', '1',  'Android','4G' );
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('13','Samsung Note 12', '2', '200 grams', '4,000 mAh', '2', '3828 x 1792','2020-10-15', '1',  'Android','4G' );
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('14','Samsung Note 13', '2', '200 grams', '4,000 mAh', '2', '4828 x 1792','2021-10-15', '1',  'Android','4G' );
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('15','Samsung Note 14', '2', '200 grams', '4,000 mAh', '2', '5828 x 1792','2022-10-15', '1', 'Android','4G' );
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('16','Samsung Note 15', '2', '200 grams', '4,000 mAh', '2', '6828 x 1792','2023-10-15', '1', 'Android','4G' );
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('17','realme C55', '2', '200 grams', '4,000 mAh', '2', '6828 x 1792','2023-10-15', '1', 'Android','4G' );
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('18','realme C33', '2', '200 grams', '4,000 mAh', '2', '6828 x 1792','2023-10-15', '1', 'Android','4G' );
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by, os, connection)
VALUE ('19','realme C30s', '2', '200 grams', '4,000 mAh', '2', '6828 x 1792','2023-10-15', '1', 'Android','4G' );

CREATE TABLE phonedetails(
phone_detail_id INT PRIMARY KEY,
phone_id INT NOT NULL,
color_id INT NOT NULL,
rom_size_id INT,
quantity INT NOT NULL DEFAULT '0',
phone_status_type INT,
price DECIMAL NOT NULL,
update_at DATETIME DEFAULT CURRENT_TIMESTAMP(),
update_by INT DEFAULT '1',
FOREIGN KEY(update_by) REFERENCES staffs(staff_id),
FOREIGN KEY(phone_id) REFERENCES phones(phone_id),
FOREIGN KEY(color_id) REFERENCES colors(color_id),
FOREIGN KEY(rom_size_id) REFERENCES romsizes(rom_size_id),
UNIQUE key pd_unique1(phone_id, color_id, rom_size_id, phone_status_type)
)engine = InnoDB;

DELIMITER $$
CREATE TRIGGER after_update_on_phonedetails AFTER UPDATE ON phonedetails
FOR EACH ROW
BEGIN
IF(new.quantity<0) THEN SIGNAL SQLSTATE '02315' SET message_text = 'Quantity cant be negative';
END IF;
END$$
DELIMITER ;

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('0', '0', '1','1', '0', '0');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('1','1', '1','1', '0', '11890000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('2','1', '1','1', '1', '9500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('3','1', '1','1', '2', '6003000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('4','1', '1','1', '3', '4010000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('5','1', '1','1', '4', '2500000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('6','1', '1','2', '0', '11500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('7','1', '1','2', '1', '9350000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('8','1', '1','2', '2', '5900000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('9','1', '1','2', '3', '3500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('10','1', '1','2', '4', '2240000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('11','1', '1','3', '0', '13800000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('12','1', '1','3', '1', '10004000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('13','1', '1','3', '2', '8560000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('14','1', '1','3', '3', '5600000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('15','1', '1','3', '4', '3250000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('16','1', '2','1', '0', '12100000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('17','1', '2','1', '1', '10800000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('18','1', '2','1', '2', '8900000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('19','1', '2','1', '3', '6798000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('20','1', '2','1', '4', '4610000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('21','1', '2','2', '0', '11900000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('22','1', '2','2', '1', '9860000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('23','1', '2','2', '2', '7580000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('24','1', '2','2', '3', '6500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('25','1', '2','2', '4', '5040000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('26','1', '2','3', '0', '11200000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('27','1', '2','3', '1', '10020000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('28','1', '2','3', '2', '9200000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('29','1', '2','3', '3', '8200000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('30','1', '2','3', '4', '7106000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('31','1', '3','1', '0', '10900000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('32','1', '3','1', '1', '9000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('33','1', '3','1', '2', '8000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('34','1', '3','1', '3', '7900000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('35','1', '3','1', '4', '6900000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('36','2', '1','1', '0', '15890000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('37','2', '1','1', '1', '12500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('38','2', '1','1', '2', '10890000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('39','2', '1','1', '3', '9890000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('40','2', '1','1', '4', '6890000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('41','3', '1','1', '0', '19490000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('42','3', '1','1', '1', '15000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('43','3', '1','1', '2', '12490000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('44','3', '1','1', '3', '10490000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('45','3', '1','1', '4', '9490000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('46','4', '1','1', '0', '21890000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('47','4', '1','1', '1', '20000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('48','4', '1','1', '2', '18900000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('49','4', '1','1', '3', '17900000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('50','4', '1','1', '4', '15890000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('51','5', '1','1', '0', '33400000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('52','5', '1','1', '1', '30000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('53','5', '1','1', '2', '28900000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('54','5', '1','1', '3', '25000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('55','5', '1','1', '4', '20000000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('56','6', '1','1', '0', '40050000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('57','6', '1','1', '1', '38500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('58','6', '1','1', '2', '34050000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('59','6', '1','1', '3', '31050000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('60','6', '1','1', '4', '28050000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('61','7', '1','1', '0', '60040000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('62','7', '1','1', '1', '58400000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('63','7', '1','1', '2', '55040000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('64','7', '1','1', '3', '51040000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('65','7', '1','1', '4', '46040000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('66','8', '1','1', '0', '75000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('67','8', '1','1', '1', '72000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('68','8', '1','1', '2', '70000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('69','8', '1','1', '3', '68000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('70','8', '1','1', '4', '60000000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('71','9', '1','1', '0', '89000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('72','9', '1','1', '1', '85000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('73','9', '1','1', '2', '80000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('74','9', '1','1', '3', '74100000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('75','9', '1','1', '4', '72000000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('76','10', '1','1', '0', '145000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('77','10', '1','1', '1', '125000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('78','10', '1','1', '2', '112500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('79','10', '1','1', '3', '108000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('80','10', '1','1', '4', '100000000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('81','11', '1','1', '0', '10390000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('82','11', '1','1', '1', '8500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('83','11', '1','1', '2', '6810000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('84','11', '1','1', '3', '5700000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('85','11', '1','1', '4', '4130000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('86','12', '1','1', '0', '12500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('87','12', '1','1', '1', '11200000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('88','12', '1','1', '2', '10500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('89','12', '1','1', '3', '8900000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('90','12', '1','1', '4', '7500000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('91','13', '1','1', '0', '19040000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('92','13', '1','1', '1', '17500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('93','13', '1','1', '2', '15000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('94','13', '1','1', '3', '13200000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('95','13', '1','1', '4', '10000000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('96','14', '1','1', '0', '25001000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('97','14', '1','1', '1', '22500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('98','14', '1','1', '2', '20101000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('99','14', '1','1', '3', '18200000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('100','14', '1','1', '4','15000000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('101','15', '1','1', '0', '30450000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('102','15', '1','1', '1', '27500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('103','15', '1','1', '2', '23450000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('104','15', '1','1', '3', '20450000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('105','15', '1','1', '4', '17450000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('106','16', '1','1', '0', '34500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('107','16', '1','1', '1', '32100000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('108','16', '1','1', '2', '30000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('109','16', '1','1', '3', '24500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('110','16', '1','1', '4', '20500000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('111','17', '1','1', '0', '4190000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('112','17', '1','1', '1', '3960000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('113','17', '1','1', '2', '3590000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('114','17', '1','1', '3', '3190000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('115','17', '1','1', '4', '2990000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('116','18', '1','1', '0', '3294000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('117','18', '1','1', '1', '3100000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('118','18', '1','1', '2', '2934000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('119','18', '1','1', '3', '2394000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('120','18', '1','1', '4', '1994000');

INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('121','19', '1','1', '0', '2296000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('122','19', '1','1', '1', '2000000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('123','19', '1','1', '2', '1896000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('124','19', '1','1', '3', '1596000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('125','19', '1','1', '4', '1196000');


CREATE TABLE imeis(
phone_imei VARCHAR(15) PRIMARY KEY,
phone_detail_id INT NOT NULL,
status INT DEFAULT '0',
FOREIGN KEY(phone_detail_id) REFERENCES phonedetails(phone_detail_id)
)engine = InnoDB;
DELIMITER $$
CREATE TRIGGER after_insert_on_imeis AFTER INSERT ON imeis
FOR EACH ROW
BEGIN
UPDATE phonedetails SET quantity = quantity+1 WHERE phone_detail_id = new.phone_detail_id;
END$$
DELIMITER ;

INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('388541254259874', '1');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('388541254259875', '1');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('388541254259876', '1');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('388541254259877', '1');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('388541254259878', '1');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('388541254259879', '1');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259880', '1');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259126', '1');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259890', '2');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259891', '2');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259892', '2');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259893', '2');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259894', '2');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259895', '3');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259896', '3');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259897', '4');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259898', '4');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259899', '5');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259100', '5');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259101', '6');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259102', '6');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259103', '7');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259104', '7');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259105', '8');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259125', '8');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259106', '8');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259107', '9');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259108', '9');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259109', '10');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259110', '11');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259111', '12');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259112', '13');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259120', '14');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259113', '14');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259114', '15');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259119', '15');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259121', '15');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541254259115', '16');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378543434259118', '16');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541545434122', '16');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532343259116', '17');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378432344259117', '17');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378543543559123', '17');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132329124', '17');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132329125', '18');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132329127', '19');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132329126', '20');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132329128', '21');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132329323', '22');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132329766', '23');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132329999', '23');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532133333333', '23');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378535032329766', '24');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378533332329999', '24');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532873333333', '25');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532136929766', '25');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132321999', '25');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532133773333', '26');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132329767', '26');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532138929999', '26');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532243333333', '27');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532432329766', '27');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132333999', '28');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('328532163333333', '28');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132159766', '28');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532178329999', '29');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532133322033', '29');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132327896', '29');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532412329999', '30');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532131453333', '30');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132378966', '30');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532417895999', '31');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532137841333', '31');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132273066', '31');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532414589999', '32');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532131444583', '32');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532457888966', '32');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532447888799', '33');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532131112233', '33');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532130147866', '33');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('324532441318799', '34');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378544131112233', '34');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532134417866', '34');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532447883459', '35');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378531233312233', '35');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532130342326', '35');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532442348799', '36');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532135772233', '36');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532164657866', '36');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378535515555799', '37');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532166666233', '37');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532788888669', '37');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532441444299', '38');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532138223355', '38');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532541454545', '38');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532435353535', '39');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532131451413', '39');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378538448484886', '39');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532222200029', '40');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532131414413', '40');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532130841186', '40');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532447820229', '41');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532102101113', '41');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532138181816', '41');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532447202019', '42');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('321041021011113', '42');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378111104444446', '42');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532440202029', '43');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532151015203', '43');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532202022226', '43');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532422020029', '44');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378551515523233', '44');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532212044446', '44');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532442020259', '45');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532131200113', '45');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378531101010226', '45');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378531012221229', '46');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378521012121023', '46');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532210210266', '46');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378544444444499', '47');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378888888888833', '47');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378666666666666', '47');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378202200220299', '48');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532132202025', '48');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532121122110', '48');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378514414415255', '49');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378022002021454', '49');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378550505124522', '49');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('365222020252322', '50');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('372102102104444', '50');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378210210120212', '50');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378251005155454', '51');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('321120120455454', '51');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378210022112212', '51');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378202210125454', '52');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378332232302222', '53');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378522021120122', '54');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('372121121221210', '55');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('372101212124455', '56');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378202121212121', '57');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378554455445454', '58');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378332213231222', '59');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532202201012', '60');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378544545545451', '61');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('354145155451545', '62');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('351055545454522', '63');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('304514505500220', '64');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('345445121212120', '65');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('375205205255450', '66');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('374455454545410', '67');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('370000000114455', '68');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378221121212212', '69');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('304510545444444', '70	');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('322022121222000', '71');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('350505010122122', '72');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('330451455545422', '73');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('355454154444777', '74');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('345451447477878', '75');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('354147477744111', '76');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('335445544545545', '77');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('344858778877878', '78');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378111111111111', '79');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('372105454554541', '80');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('372312447487888', '81');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378323154888882', '82');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('352015278754212', '83');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('354415444444445', '84');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('372222222444444', '85');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('354585421222221', '86');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('355587451221212', '87');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('366516587523565', '88');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('354514747777451', '89');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532155454541', '90');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378577445414447', '91');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378535544577777', '92');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378535445454552', '93');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('377447777777777', '94');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('311112222222111', '95');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('354154744777444', '96');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('375415415415454', '97');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('371414554541554', '98');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('372133333333333', '99');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('344444444447777', '100');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532120544145', '101');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378554455144771', '102');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('341141441444141', '103');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378554145151112', '104');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378154154151454', '105');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('371451541547777', '106');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532541544553', '107');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378532747414116', '108');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378541787445459', '109');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378574411554553', '110');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378551451541556', '111');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('371141444444449', '112');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378888555522113', '113');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378535151541756', '114');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('366666666666669', '115');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('385481565155153', '116');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('315515418151226', '117');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378535415454549', '118');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378566516516563', '119');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('376165165656516', '120');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378514451515451', '121');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('374999999999999', '122');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('325141477775451', '123');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378222222555451', '124');
INSERT INTO imeis(phone_imei, phone_detail_id) VALUE('378365788415451', '125');

CREATE TABLE orders(
order_id VARCHAR(15) PRIMARY KEY NOT NULL,
seller_id INT NOT NULL,
accountant_id INT DEFAULT '2',
customer_id INT NOT NULL,
order_status INT DEFAULT '0',
create_at DATETIME DEFAULT CURRENT_TIMESTAMP(),
update_at DATETIME,
payment_method VARCHAR(30) DEFAULT 'Not Have',
FOREIGN KEY(seller_id) REFERENCES staffs(staff_id),
FOREIGN KEY(accountant_id) REFERENCES staffs(staff_id),
FOREIGN KEY(customer_id) REFERENCES customers(customer_id)
)engine = InnoDB;

CREATE TABLE orderdetails(
order_id VARCHAR(15) NOT NULL,
phone_imei VARCHAR(15) NOT NULL,
FOREIGN KEY (order_id) REFERENCES orders(order_id),
FOREIGN KEY (phone_imei) REFERENCES imeis(phone_imei),
constraint unique(order_id, phone_imei)
)engine = InnoDB;


DELIMITER $$
CREATE TRIGGER after_insert_on_orderdetails AFTER INSERT ON OrderDetails
FOR EACH ROW
BEGIN
UPDATE Imeis SET status = '1' WHERE phone_imei = new.phone_imei;
END$$
DELIMITER ;

DELIMITER $$
CREATE TRIGGER after_update_on_imeis_from_notexport_to_export AFTER UPDATE ON imeis
FOR EACH ROW
BEGIN
if(new.status = 1 and new.status!=old.status) then
UPDATE phonedetails SET quantity = quantity - 1 WHERE phone_detail_id = new.phone_detail_id;
end if;
END$$
DELIMITER ;

delimiter $$
create trigger after_update_on_imeis_from_export_to_notexport after update on imeis
for each row
begin
declare phoneimei varchar(15);

select i.phone_imei into phoneimei from imeis i
inner join orderdetails od on i.phone_imei = od.phone_imei 
where i.phone_imei = new.phone_imei;

if(new.status != old.status and new.status = 0) then 
update phonedetails set quantity = quantity+1 where phone_detail_id = new.phone_detail_id;
delete from orderdetails where phone_imei = phoneimei;
end if;
end$$
delimiter ;



CREATE TABLE discountpolicies(
-- thong tin co ban cua discount
policy_id INT AUTO_INCREMENT PRIMARY KEY,
title VARCHAR(100) NOT NULL,
from_date DATETIME NOT NULL,
to_date DATETIME NOT NULL,
-- thong tin cu the ve discount
-- discount theo payment/ theo order
payment_method VARCHAR(20) default 'Not Have', -- should be updated later
maximum_purchase_amount DECIMAL default 0,
minimum_purchase_amount DECIMAL default 0,
discount_price DECIMAL default 0,
discount_rate DECIMAL default 0, 
-- discount theo chinh sach thu cu
phone_detail_id INT default '0',
money_supported DECIMAL default 0,
-- cac thong tin con lai
create_by INT NOT NULL,
create_at DATETIME DEFAULT CURRENT_TIMESTAMP(),
update_by INT DEFAULT '1',
update_at DATETIME DEFAULT CURRENT_TIMESTAMP(),
description VARCHAR(200) default 'Not Have',
FOREIGN KEY (phone_detail_id) REFERENCES phonedetails(phone_detail_id),
FOREIGN KEY (create_by) REFERENCES staffs(staff_id),
FOREIGN KEY (update_by) REFERENCES staffs(staff_id)
)engine = InnoDB;
-- Giam gia Payment method
-- VNPay: 
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Discount for Order have Paymentmethod VNPay and TotalDue over 500.000VND', '2023-07-07', '2024-07-07','500000','3500000' , '100000','VNPay', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Discount for Order have Paymentmethod VNPay and TotalDue over 3.500.000VND', '2023-07-07', '2024-07-07','3500001','5000000' , '125000','VNPay', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Discount for Order have Paymentmethod VNPay and TotalDue over 5.000.000VND', '2023-07-07', '2024-07-07','5000001','1000000000' , '150000','VNPay', '2');
-- Banking:
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Discount for Order have Paymentmethod Banking and TotalDue over 1.000.000VND', '2023-07-07', '2024-07-07','1000001','5000000' , '250000','Banking', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Discount for Order have Paymentmethod Banking and TotalDue over 5.000.000VND', '2023-07-07', '2024-07-07','5000001','10000000' , '300000','Banking', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Discount for Order have Paymentmethod Banking and TotalDue over 10.000.000VND', '2023-07-07', '2024-07-07','10000001','1000000000' , '400000','Banking', '2');
-- Cash:
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Discount for Order have Paymentmethod Cash and TotalDue over 5.000.000VND', '2023-07-07', '2024-07-07','5000001','10000000' , '250000','Cash', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Discount for Order have Paymentmethod Cash and TotalDue over 10.000.000VND', '2023-07-07', '2024-07-07','10000001','15000000' , '300000','Cash', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Discount for Order have Paymentmethod Cash and TotalDue over 15.000.000VND', '2023-07-07', '2024-07-07','15000001','1000000000' , '400000','Cash', '2');
-- Giam gia Paying offer
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price, create_by) VALUE('Discount For Order have TotalDue from 500.000VND to 3.500.000VND', '2023-07-07', '2024-07-07','500001','3500000' , '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price, create_by) VALUE('Discount For Order have TotalDue from 3.500.000VND to 5.000.000VND', '2023-07-07', '2024-07-07','3500001','5000000' , '250000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price, create_by) VALUE('Discount For Order have TotalDue over 5.000.000VND', '2023-07-07', '2024-07-07','5000001','1000000000' , '500000', '2');
-- Ho tro thu cu
-- Dien thoai Iphone
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '1', '600000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '2', '600000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '3', '600000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '4', '600000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '5', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '6', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '7', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '8', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '9', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '10', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '11', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '12', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '13', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '14', '900000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '15', '900000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '16', '900000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '17', '900000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '18', '800000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '19', '700000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '20', '800000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '21', '800000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '22', '800000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '23', '900000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '24', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '25', '700000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '26', '700000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '27', '700000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '28', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '29', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '30', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '31', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '32', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '33', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '34', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '35', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '36', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '37', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '38', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '39', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '40', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '41', '700000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '42', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '43', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '44', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '45', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '46', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '47', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '48', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '49', '300000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '50', '600000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '51', '700000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '52', '800000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '53', '900000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '54', '150000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '55', '150000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '56', '160000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '57', '180000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '58', '170000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '59', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '60', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '61', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '62', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '63', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '64', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '65', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '66', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '67', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '68', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '69', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '70', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '71', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '72', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '73', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '74', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '75', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '76', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '77', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '78', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '79', '1000000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Iphone', '2023-07-07', '2024-07-07', '80', '1000000', '2');
-- Dien thoai Samsung
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '81', '250000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '82', '350000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '83', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '84', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '85', '300000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '86', '250000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '87', '125000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '88', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '89', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '90', '350000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '91', '250000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '92', '150000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '93', '200000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '94', '500000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '95', '450000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '96', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '97', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '98', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '99', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '100', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '101', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '102', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '103', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '104', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '105', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '106', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '107', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '108', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '109', '400000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for Samsung', '2023-07-07', '2024-07-07', '110', '400000', '2');
-- Dien thoai readlme
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for realme', '2023-07-07', '2024-07-07', '111', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for realme', '2023-07-07', '2024-07-07', '112', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for realme', '2023-07-07', '2024-07-07', '113', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for realme', '2023-07-07', '2024-07-07', '114', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for realme', '2023-07-07', '2024-07-07', '115', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for realme', '2023-07-07', '2024-07-07', '116', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for realme', '2023-07-07', '2024-07-07', '117', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for realme', '2023-07-07', '2024-07-07', '118', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, money_supported, create_by) VALUE('TradeIn Policy for realme', '2023-07-07', '2024-07-07', '119', '100000', '2');


CREATE TABLE discountpolicydetails(
policy_detail_id INT AUTO_INCREMENT PRIMARY KEY,
policy_id INT NOT NULL,
order_id VARCHAR(15) NOT NULL,
FOREIGN KEY (policy_id) REFERENCES discountpolicies(policy_id),
FOREIGN KEY (order_id) REFERENCES orders(order_id)
)engine = InnoDB;

-- SELECT P.*, PD.*, I.* FROM orders O
--                 INNER JOIN orderdetails OD ON O.order_id = OD.order_id
--                 INNER JOIN imeis I ON I.phone_imei = OD.phone_imei
--                 INNER JOIN phonedetails PD ON I.phone_detail_id = PD.phone_detail_id
--                 INNER JOIN phones P ON PD.phone_id = P.phone_id
INSERT INTO Orders(order_id, seller_id, accountant_id, customer_id, create_at, order_status) VALUES ('A798EC16225E', 3, 2, 1, STR_TO_DATE("08-01-2023 16:40:10", "%m-%d-%Y %H:%i:%s"), 3);
INSERT INTO Orders(order_id, seller_id, accountant_id, customer_id, create_at, order_status) VALUES ('D2BD913A83B4', 2, 2, 2, STR_TO_DATE("08-02-2023 17:40:10", "%m-%d-%Y %H:%i:%s"), 3);
INSERT INTO Orders(order_id, seller_id, accountant_id, customer_id, create_at, order_status) VALUES ('D2B3233A83B4', 2, 3, 2, STR_TO_DATE("07-31-2023 15:40:10", "%m-%d-%Y %H:%i:%s"), 3);
INSERT INTO Orders(order_id, seller_id, accountant_id, customer_id, create_at, order_status) VALUES ('F2B1233A83C6', 2, 3, 2, STR_TO_DATE("08-01-2023 19:40:10", "%m-%d-%Y %H:%i:%s"), 3);
INSERT INTO Orders(order_id, seller_id, accountant_id, customer_id, create_at, order_status) VALUES ('T2B1233A83V9', 2, 3, 2, STR_TO_DATE("08-06-2023 19:40:10", "%m-%d-%Y %H:%i:%s"), 3);
INSERT INTO Orders(order_id, seller_id, accountant_id, customer_id, create_at, order_status) VALUES ('P2B1222A83T2', 2, 3, 2, STR_TO_DATE("08-12-2023 19:40:10", "%m-%d-%Y %H:%i:%s"), 3);
INSERT INTO OrderDetails(order_id, phone_imei) VALUES ('F2B1233A83C6', 378432344259117);
INSERT INTO OrderDetails(order_id, phone_imei) VALUES ('T2B1233A83V9', 378532343259116);
INSERT INTO OrderDetails(order_id, phone_imei) VALUES ('A798EC16225E', 378541254259880);
INSERT INTO OrderDetails(order_id, phone_imei) VALUES ('D2BD913A83B4', 378541254259890);
INSERT INTO OrderDetails(order_id, phone_imei) VALUES ('D2B3233A83B4', 378541254259891);
INSERT INTO OrderDetails(order_id, phone_imei) VALUES ('P2B1222A83T2', 378541254259121);


-- CREATE AN SUB ACCOUNT TO DATABASE 
CREATE USER IF NOT exists 'something'@'localhost' IDENTIFIED BY '123456';
GRANT ALL PRIVILEGES ON phonestore.* TO 'something'@'localhost' WITH GRANT OPTION;