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
VALUE ('Bui Tien Dat', '0789456123','Hanoi','accountant','e99a18c428cb38d5f260853678922e03',1, 1);
INSERT INTO staffs(name, phone_number, address, user_name, password, role, status)
VALUE ('Tran Tien Anh', '0902126092','Hanoi','seller01','e99a18c428cb38d5f260853678922e03',0, 1);

CREATE TABLE customers(
customer_id INT AUTO_INCREMENT PRIMARY KEY,
name VARCHAR(50) NOT NULL,
phone_number VARCHAR(10) NOT NULL,
address VARCHAR(50)
)engine = InnoDB;
INSERT INTO customers(name, phone_number, address) VALUE('Yua Mikami', '0254136578', 'Quat Lam');
INSERT INTO customers(name, phone_number, address) VALUE('Erichio Masharo', '0254136548', 'Quat Lam');
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
INSERT INTO phones(phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by) 
VALUE('1','Nokia 1280', '2', '250g', '1080mAh', '1','500x200', '1998-12-12', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('2', 'Iphone 11', '1', '194 grams', '3,110 mAh', '2', '828 x 1792','2019-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('3', 'Iphone 12', '1', '194 grams', '3,110 mAh', '2', '1828 x 1792','2019-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('4', 'Iphone 13', '1', '194 grams', '3,110 mAh', '2', '2828 x 1792','2029-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('5', 'Iphone 14', '1', '194 grams', '3,110 mAh', '2', '3828 x 1792','2039-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('6', 'Iphone 15', '1', '194 grams', '3,110 mAh', '2', '4828 x 1792','2049-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('7','Iphone 16', '1', '194 grams', '3,110 mAh', '2', '5828 x 1792','2059-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('8', 'Iphone 17', '1', '194 grams', '3,110 mAh', '2', '6828 x 1792','2069-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('9', 'Iphone 18', '1', '194 grams', '3,110 mAh', '2', '7828 x 1792','2079-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('10','Iphone 19', '1', '194 grams', '3,110 mAh', '2', '8828 x 1792','2089-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('11','Iphone XX', '1', '194 grams', '3,110 mAh', '2', '9828 x 1792','2099-09-20', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('12', 'Samsung Note 10', '2', '200 grams', '4,000 mAh', '2', '1828 x 1792','2018-10-15', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('13','Samsung Note 11', '2', '200 grams', '4,000 mAh', '2', '2828 x 1792','2019-10-15', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('14','Samsung Note 12', '2', '200 grams', '4,000 mAh', '2', '3828 x 1792','2020-10-15', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('15','Samsung Note 13', '2', '200 grams', '4,000 mAh', '2', '4828 x 1792','2021-10-15', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('16','Samsung Note 14', '2', '200 grams', '4,000 mAh', '2', '5828 x 1792','2022-10-15', '1');
INSERT INTO phones (phone_id, phone_name, brand_id, weight, battery_capacity, sim_slot,screen, release_date, create_by)
VALUE ('17','Samsung Note 15', '2', '200 grams', '4,000 mAh', '2', '6828 x 1792','2023-10-15', '1');


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
UNIQUE key pd_unique1(phone_id, color_id, rom_size_id)
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
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('1','1', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('2','1', '1','2', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('3','1', '1','3', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('4','1', '2','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('5','1', '2','2', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('6','1', '2','3', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('7','1', '3','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('8','2', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('9','3', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('10','4', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('11','5', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('12','6', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('13','7', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('14','8', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('15','9', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('16','10', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('17','11', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('18','12', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('19','13', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('20','14', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('21','15', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('22','16', '1','1', '0', '500000');
INSERT INTO phonedetails(phone_detail_id, phone_id, color_id,rom_size_id, phone_status_type, price) VALUE('23','17', '1','1', '0', '500000');

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

INSERT INTO Orders(order_id, seller_id, customer_id) VALUES ('A798EC16225E', 3, 1);
INSERT INTO Orders(order_id, seller_id, customer_id) VALUES ('D2BD913A83B4', 2, 2);

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

INSERT INTO OrderDetails(order_id, phone_imei) VALUES ('A798EC16225E', 378541254259880);
INSERT INTO OrderDetails(order_id, phone_imei) VALUES ('D2BD913A83B4', 378541254259890);



CREATE TABLE discountpolicies(
-- thong tin co ban cua discount
policy_id INT AUTO_INCREMENT PRIMARY KEY,
title VARCHAR(100) NOT NULL,
from_date DATETIME NOT NULL,
to_date DATETIME NOT NULL,
-- thong tin cu the ve discount
-- discount theo payment/ theo order
payment_method VARCHAR(20) default 'Not Have',
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
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, discount_price, create_by) VALUE('Giam gia cho dien thoai Nokia 1280 model 1', '2023-07-07', '2024-07-07', '1', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, phone_detail_id, discount_price, create_by) VALUE('Giam gia cho dien thoai Nokia 1280 model 2', '2023-07-07', '2024-07-07', '2', '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price, create_by) VALUE('Giam gia tong chi tren 500000 cua order', '2023-07-07', '2024-07-07','500000','3500000' , '100000', '2');
INSERT INTO discountpolicies(title, from_date, to_date, minimum_purchase_amount, maximum_purchase_amount, discount_price,payment_method, create_by) VALUE('Giam gia tong chi tren 500000 cho VNPay loai 1', '2023-07-07', '2024-07-07','500000','3500000' , '100000','VNPay', '2');
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