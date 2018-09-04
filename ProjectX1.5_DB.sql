
drop table AlcoholProduct;
drop table AlcoholCategory;
drop table Sales;
drop table BuyingHistoryItems;
drop table BuyingHistory;
drop table CreditCards;
drop table CigaresProducts;
drop table CigarStrength;
drop table CigaresWrapper;
drop table CigarBrands;
drop table Accessories;
drop table AccessoryType;
drop table BundleInfo;
drop table Bundles;
drop table Product;
drop table Customer;
drop table Country;

create table AlcoholCategory(
	id int primary key IDENTITY (1, 1) NOT NULL,
	categoryName varchar(50) unique not null,
	pic varchar(500) not null
);

create table Country(
	countryId int IDENTITY (1, 1) NOT NULL,
	pic varchar(500) not null,
	countryName varchar(50) primary key
);

create table Product(
	productID int primary key identity(1,1) not null,
	productName varchar(50) not null,
	price int not null,
	info varchar(200),
	pic varchar(500) not null
);
create table AlcoholProduct(
    productID int primary key references product(productID),
    categoryID int references AlcoholCategory (id),
    volume int not null,
    origin varchar(50) references Country(countryName)
);

create table Sales
(
	productID int not null references product(productID) primary key,
	saleName varchar(200) not null,
	startDate date not null,
	endDate date not null,
	discount int not null,
	info varchar(300)
);

create table Customer
(
	customerId nvarchar(128) primary key references AspNetUsers(id) on delete cascade on update no action,
	customerName varchar(20) not null,
	customerLastName varchar(20) not null,
	customerphoneNumber	varchar(20),
	customerCountry varchar(50) not null references Country(countryName),
	customerCity varchar(50)
);
create table BuyingHistory
(
	buyingId int primary key IDENTITY (1, 1) NOT NULL,
	userId nvarchar(128) references Customer(customerId) on delete cascade on update no action,
	firstName varchar(100) not null,
	lastName varchar(100) not null,
	phoneNumber varchar(15),
	postalCode varchar(20) not null,
	creditCardID varchar(50) not null,
	buyingDate varchar(50) not null,
	shippmentCountry varchar(50) not null references Country(countryName),
	shippmentCity varchar(150) not null,
	shippmentAddress varchar(150) not null,
	totalPrice int not null
);
create table BuyingHistoryItems
(
	buyingId int not null references BuyingHistory(buyingId),
	productID int not null references product(productID) on delete cascade,
	bundleId int not null references Bundles(bundleId) on delete cascade,
	quantity int not null,
	primary key(buyingId, productID,bundleId)
);
create table CreditCards(
	creditCardID varchar(300) primary key not null,
	creditType varchar(50) not null,
	ownerName varchar(50) not null,
	expirationMonth varchar(10) not null,
	expirationYear varchar(10) not null,
	securityDigits varchar(3) not null,
	userId nvarchar(128) references customer(customerId)
);
create table CigaresWrapper
(
	wrapperName varchar(50) primary Key,
	color varchar(100) not null
);
create table CigarStrength
(
	id int primary key IDENTITY (1, 1) NOT NULL,
	strengthName varchar(50) unique not null,
);
create table CigarBrands
(
	id int primary key IDENTITY (1, 1) NOT NULL,
	brandName varchar(100) not null unique,
	pic varchar(500) not null
);
create table AccessoryType
(
	id int primary key IDENTITY (1, 1) NOT NULL,
	typeName varchar(50) unique not null,
	pic varchar(500)
);
create table CigaresProducts
(
	productID int primary key references product(productID),
	bundle int not null,
	strengthID int not null references cigarStrength(id),
	origin varchar(50) references Country(countryName),
	wrapperType varchar(50) references CigaresWrapper(wrapperName) on delete cascade,
	cigarBrand int not null references CigarBrands(id) on delete cascade
);

create table Accessories
(
	productID int primary key references product(productID),
	accessoryType int not null references accessoryType(id),
	description varchar(500)
);

create table Bundles
(
	bundleId INT PRIMARY KEY IDENTITY (1, 1) NOT NULL,
	bundleName varchar(50) not null,
	info varchar(500),
	price int not null
);

create Table BundleInfo
(
	bundleId int references bundles(bundleId),
	productID int references product(productID),
	quantity int not null,
	primary key(bundleId,productID)
);

create table WatchList
(
	customerId nvarchar(128) references Customer(customerId) on delete cascade,
	productID int references product(productID) on delete cascade,
	bundleId int references bundles(bundleId) on delete cascade, 
	note varchar(500),
	primary key(customerId,productID,bundleId)
);

insert into AlcoholCategory (categoryName,pic) values('Whisky','https://www.mashkaot.co.il/files/catalog_cat/thumb/viski_bokaly_led_kubiki.jpg');
insert into AlcoholCategory (categoryName,pic) values('Koniak','https://www.mashkaot.co.il/files/catalog_cat/thumb/cognac_glass_comandon_1.jpg');
insert into AlcoholCategory (categoryName,pic) values('Liquor','https://www.mashkaot.co.il/files/catalog_cat/thumb/samye_vkusnye_likery.jpg');
insert into AlcoholCategory (categoryName,pic) values('Jin','https://www.mashkaot.co.il/files/catalog_cat/thumb/The_GBC_Gin_Basil_Cucumber_3.jpg');
insert into AlcoholCategory (categoryName,pic) values('Rom','https://www.mashkaot.co.il/files/catalog_cat/thumb/pravila_upotrebleniya_roma_ch_1.jpg');
insert into AlcoholCategory (categoryName,pic) values('Vodka','https://www.mashkaot.co.il/files/catalog_cat/thumb/image_1923903.jpeg');
insert into AlcoholCategory (categoryName,pic) values('Anis','https://www.mashkaot.co.il/files/catalog_cat/thumb/81a98139b79f04a5be4607b3871f1db5.jpg');
insert into AlcoholCategory (categoryName,pic) values('Tequila','https://www.mashkaot.co.il/files/catalog_cat/thumb/tequila.jpg');

insert into CigarBrands (brandName,pic) values('Bolivar','https://www.finestcubancigars.com/media/catalog/category/menu_brand_bolivar.jpg');
insert into CigarBrands (brandName,pic) values('Cohiba','https://www.finestcubancigars.com/media/catalog/category/menu_brand_cohiba.jpg');
insert into CigarBrands (brandName,pic) values('Montecristo','https://www.finestcubancigars.com/media/catalog/category/menu_brand_montecristo.jpg');
insert into CigarBrands (brandName,pic) values('Ashton Heritage','http://brandongaille.com/wp-content/uploads/2014/01/Ashton-Heritage-Company-Logo.jpg');
insert into CigarBrands (brandName,pic) values('El Baton','http://brandongaille.com/wp-content/uploads/2014/01/El-Baton-Company-Logo.jpg');
insert into CigarBrands (brandName,pic) values('Kristoff','http://brandongaille.com/wp-content/uploads/2014/01/Kristoff-Company-Logo.jpg');
insert into CigarBrands (brandName,pic) values('Nat Sherman','http://brandongaille.com/wp-content/uploads/2014/01/Nat-Sherman-Company-Logo.jpg');
insert into CigarBrands (brandName,pic) values('Padron Cigars','http://brandongaille.com/wp-content/uploads/2014/01/Padron-Cigars-Company-Logo.jpg');
insert into CigarBrands (brandName,pic) values('Partagás','http://brandongaille.com/wp-content/uploads/2014/01/Partag%C3%A1s-Company-Logo.jpg');
insert into CigarBrands (brandName,pic) values('Romeo y Julieta','http://brandongaille.com/wp-content/uploads/2014/01/Romeo-y-Julieta-Company-Logo.jpg');
insert into CigarBrands (brandName,pic) values('Tabacalera Cubana','http://brandongaille.com/wp-content/uploads/2014/01/Tabacalera-Cubana-Company-Logo.jpg');
insert into CigarBrands (brandName,pic) values('Tatuaje','http://brandongaille.com/wp-content/uploads/2014/01/Tatuaje-Company-Logo.jpg');

insert into cigarStrength (strengthName) values('Light');
insert into cigarStrength (strengthName) values('Light to medium');
insert into cigarStrength (strengthName) values('Medium');
insert into cigarStrength (strengthName) values('Medium to full');
insert into cigarStrength (strengthName) values('Full');

insert into AccessoryType (typeName) values('Glasses');
insert into AccessoryType (typeName) values('For bottles');

insert into CigaresWrapper values('Candela Double Claro', 'very light, slightly greenish');
insert into CigaresWrapper values('Claro', 'yellowish');
insert into CigaresWrapper values('Colorado Claro', 'medium brown');
insert into CigaresWrapper values('Colorado Rosado', 'reddish-brown');
insert into CigaresWrapper values('Colorado Maduro', 'darker brown');
insert into CigaresWrapper values('Maduro', 'very dark brown');
insert into CigaresWrapper values('Oscuro Double Maduro', 'black');

INSERT INTO country (pic,countryname) VALUES ('AF', 'Afghanistan');
INSERT INTO country (pic,countryname) VALUES ('AL', 'Albania');
INSERT INTO country (pic,countryname) VALUES ('DZ', 'Algeria');
INSERT INTO country (pic,countryname) VALUES ('DS', 'American Samoa');
INSERT INTO country (pic,countryname) VALUES ('AD', 'Andorra');
INSERT INTO country (pic,countryname) VALUES ('AO', 'Angola');
INSERT INTO country (pic,countryname) VALUES ('AI', 'Anguilla');
INSERT INTO country (pic,countryname) VALUES ('AQ', 'Antarctica');
INSERT INTO country (pic,countryname) VALUES ('AG', 'Antigua and Barbuda');
INSERT INTO country (pic,countryname) VALUES ('AR', 'Argentina');
INSERT INTO country (pic,countryname) VALUES ('AM', 'Armenia');
INSERT INTO country (pic,countryname) VALUES ('AW', 'Aruba');
INSERT INTO country (pic,countryname) VALUES ('AU', 'Australia');
INSERT INTO country (pic,countryname) VALUES ('AT', 'Austria');
INSERT INTO country (pic,countryname) VALUES ('AZ', 'Azerbaijan');
INSERT INTO country (pic,countryname) VALUES ('BS', 'Bahamas');
INSERT INTO country (pic,countryname) VALUES ('BH', 'Bahrain');
INSERT INTO country (pic,countryname) VALUES ('BD', 'Bangladesh');
INSERT INTO country (pic,countryname) VALUES ('BB', 'Barbados');
INSERT INTO country (pic,countryname) VALUES ('BY', 'Belarus');
INSERT INTO country (pic,countryname) VALUES ('BE', 'Belgium');
INSERT INTO country (pic,countryname) VALUES ('BZ', 'Belize');
INSERT INTO country (pic,countryname) VALUES ('BJ', 'Benin');
INSERT INTO country (pic,countryname) VALUES ('BM', 'Bermuda');
INSERT INTO country (pic,countryname) VALUES ('BT', 'Bhutan');
INSERT INTO country (pic,countryname) VALUES ('BO', 'Bolivia');
INSERT INTO country (pic,countryname) VALUES ('BA', 'Bosnia and Herzegovina');
INSERT INTO country (pic,countryname) VALUES ('BW', 'Botswana');
INSERT INTO country (pic,countryname) VALUES ('BV', 'Bouvet Island');
INSERT INTO country (pic,countryname) VALUES ('BR', 'Brazil');
INSERT INTO country (pic,countryname) VALUES ('IO', 'British Indian Ocean Territory');
INSERT INTO country (pic,countryname) VALUES ('BN', 'Brunei Darussalam');
INSERT INTO country (pic,countryname) VALUES ('BG', 'Bulgaria');
INSERT INTO country (pic,countryname) VALUES ('BF', 'Burkina Faso');
INSERT INTO country (pic,countryname) VALUES ('BI', 'Burundi');
INSERT INTO country (pic,countryname) VALUES ('KH', 'Cambodia');
INSERT INTO country (pic,countryname) VALUES ('CM', 'Cameroon');
INSERT INTO country (pic,countryname) VALUES ('CA', 'Canada');
INSERT INTO country (pic,countryname) VALUES ('CV', 'Cape Verde');
INSERT INTO country (pic,countryname) VALUES ('KY', 'Cayman Islands');
INSERT INTO country (pic,countryname) VALUES ('CF', 'Central African Republic');
INSERT INTO country (pic,countryname) VALUES ('TD', 'Chad');
INSERT INTO country (pic,countryname) VALUES ('CL', 'Chile');
INSERT INTO country (pic,countryname) VALUES ('CN', 'China');
INSERT INTO country (pic,countryname) VALUES ('CX', 'Christmas Island');
INSERT INTO country (pic,countryname) VALUES ('CC', 'Cocos (Keeling) Islands');
INSERT INTO country (pic,countryname) VALUES ('CO', 'Colombia');
INSERT INTO country (pic,countryname) VALUES ('KM', 'Comoros');
INSERT INTO country (pic,countryname) VALUES ('CG', 'Congo');
INSERT INTO country (pic,countryname) VALUES ('CK', 'Cook Islands');
INSERT INTO country (pic,countryname) VALUES ('CR', 'Costa Rica');
INSERT INTO country (pic,countryname) VALUES ('HR', 'Croatia (Hrvatska)');
INSERT INTO country (pic,countryname) VALUES ('CU', 'Cuba');
INSERT INTO country (pic,countryname) VALUES ('CY', 'Cyprus');
INSERT INTO country (pic,countryname) VALUES ('CZ', 'Czech Republic');
INSERT INTO country (pic,countryname) VALUES ('DK', 'Denmark');
INSERT INTO country (pic,countryname) VALUES ('DJ', 'Djibouti');
INSERT INTO country (pic,countryname) VALUES ('DM', 'Dominica');
INSERT INTO country (pic,countryname) VALUES ('DO', 'Dominican Republic');
INSERT INTO country (pic,countryname) VALUES ('TP', 'East Timor');
INSERT INTO country (pic,countryname) VALUES ('EC', 'Ecuador');
INSERT INTO country (pic,countryname) VALUES ('EG', 'Egypt');
INSERT INTO country (pic,countryname) VALUES ('SV', 'El Salvador');
INSERT INTO country (pic,countryname) VALUES ('GQ', 'Equatorial Guinea');
INSERT INTO country (pic,countryname) VALUES ('ER', 'Eritrea');
INSERT INTO country (pic,countryname) VALUES ('EE', 'Estonia');
INSERT INTO country (pic,countryname) VALUES ('ET', 'Ethiopia');
INSERT INTO country (pic,countryname) VALUES ('FK', 'Falkland Islands (Malvinas)');
INSERT INTO country (pic,countryname) VALUES ('FO', 'Faroe Islands');
INSERT INTO country (pic,countryname) VALUES ('FJ', 'Fiji');
INSERT INTO country (pic,countryname) VALUES ('FI', 'Finland');
INSERT INTO country (pic,countryname) VALUES ('FR', 'France');
INSERT INTO country (pic,countryname) VALUES ('FX', 'France, Metropolitan');
INSERT INTO country (pic,countryname) VALUES ('GF', 'French Guiana');
INSERT INTO country (pic,countryname) VALUES ('PF', 'French Polynesia');
INSERT INTO country (pic,countryname) VALUES ('TF', 'French Southern Territories');
INSERT INTO country (pic,countryname) VALUES ('GA', 'Gabon');
INSERT INTO country (pic,countryname) VALUES ('GM', 'Gambia');
INSERT INTO country (pic,countryname) VALUES ('GE', 'Georgia');
INSERT INTO country (pic,countryname) VALUES ('DE', 'Germany');
INSERT INTO country (pic,countryname) VALUES ('GH', 'Ghana');
INSERT INTO country (pic,countryname) VALUES ('GI', 'Gibraltar');
INSERT INTO country (pic,countryname) VALUES ('GK', 'Guernsey');
INSERT INTO country (pic,countryname) VALUES ('GR', 'Greece');
INSERT INTO country (pic,countryname) VALUES ('GL', 'Greenland');
INSERT INTO country (pic,countryname) VALUES ('GD', 'Grenada');
INSERT INTO country (pic,countryname) VALUES ('GP', 'Guadeloupe');
INSERT INTO country (pic,countryname) VALUES ('GU', 'Guam');
INSERT INTO country (pic,countryname) VALUES ('GT', 'Guatemala');
INSERT INTO country (pic,countryname) VALUES ('GN', 'Guinea');
INSERT INTO country (pic,countryname) VALUES ('GW', 'Guinea-Bissau');
INSERT INTO country (pic,countryname) VALUES ('GY', 'Guyana');
INSERT INTO country (pic,countryname) VALUES ('HT', 'Haiti');
INSERT INTO country (pic,countryname) VALUES ('HM', 'Heard and Mc Donald Islands');
INSERT INTO country (pic,countryname) VALUES ('HN', 'Honduras');
INSERT INTO country (pic,countryname) VALUES ('HK', 'Hong Kong');
INSERT INTO country (pic,countryname) VALUES ('HU', 'Hungary');
INSERT INTO country (pic,countryname) VALUES ('IS', 'Iceland');
INSERT INTO country (pic,countryname) VALUES ('IN', 'India');
INSERT INTO country (pic,countryname) VALUES ('IM', 'Isle of Man');
INSERT INTO country (pic,countryname) VALUES ('ID', 'Indonesia');
INSERT INTO country (pic,countryname) VALUES ('IR', 'Iran (Islamic Republic of)');
INSERT INTO country (pic,countryname) VALUES ('IQ', 'Iraq');
INSERT INTO country (pic,countryname) VALUES ('IE', 'Ireland');
INSERT INTO country (pic,countryname) VALUES ('IL', 'Israel');
INSERT INTO country (pic,countryname) VALUES ('IT', 'Italy');
INSERT INTO country (pic,countryname) VALUES ('CI', 'Ivory Coast');
INSERT INTO country (pic,countryname) VALUES ('JE', 'Jersey');
INSERT INTO country (pic,countryname) VALUES ('JM', 'Jamaica');
INSERT INTO country (pic,countryname) VALUES ('JP', 'Japan');
INSERT INTO country (pic,countryname) VALUES ('JO', 'Jordan');
INSERT INTO country (pic,countryname) VALUES ('KZ', 'Kazakhstan');
INSERT INTO country (pic,countryname) VALUES ('KE', 'Kenya');
INSERT INTO country (pic,countryname) VALUES ('KI', 'Kiribati');
INSERT INTO country (pic,countryname) VALUES ('KP', 'Korea, Democratic People''s Republic of');
INSERT INTO country (pic,countryname) VALUES ('KR', 'Korea, Republic of');
INSERT INTO country (pic,countryname) VALUES ('XK', 'Kosovo');
INSERT INTO country (pic,countryname) VALUES ('KW', 'Kuwait');
INSERT INTO country (pic,countryname) VALUES ('KG', 'Kyrgyzstan');
INSERT INTO country (pic,countryname) VALUES ('LA', 'Lao People''s Democratic Republic');
INSERT INTO country (pic,countryname) VALUES ('LV', 'Latvia');
INSERT INTO country (pic,countryname) VALUES ('LB', 'Lebanon');
INSERT INTO country (pic,countryname) VALUES ('LS', 'Lesotho');
INSERT INTO country (pic,countryname) VALUES ('LR', 'Liberia');
INSERT INTO country (pic,countryname) VALUES ('LY', 'Libyan Arab Jamahiriya');
INSERT INTO country (pic,countryname) VALUES ('LI', 'Liechtenstein');
INSERT INTO country (pic,countryname) VALUES ('LT', 'Lithuania');
INSERT INTO country (pic,countryname) VALUES ('LU', 'Luxembourg');
INSERT INTO country (pic,countryname) VALUES ('MO', 'Macau');
INSERT INTO country (pic,countryname) VALUES ('MK', 'Macedonia');
INSERT INTO country (pic,countryname) VALUES ('MG', 'Madagascar');
INSERT INTO country (pic,countryname) VALUES ('MW', 'Malawi');
INSERT INTO country (pic,countryname) VALUES ('MY', 'Malaysia');
INSERT INTO country (pic,countryname) VALUES ('MV', 'Maldives');
INSERT INTO country (pic,countryname) VALUES ('ML', 'Mali');
INSERT INTO country (pic,countryname) VALUES ('MT', 'Malta');
INSERT INTO country (pic,countryname) VALUES ('MH', 'Marshall Islands');
INSERT INTO country (pic,countryname) VALUES ('MQ', 'Martinique');
INSERT INTO country (pic,countryname) VALUES ('MR', 'Mauritania');
INSERT INTO country (pic,countryname) VALUES ('MU', 'Mauritius');
INSERT INTO country (pic,countryname) VALUES ('TY', 'Mayotte');
INSERT INTO country (pic,countryname) VALUES ('MX', 'Mexico');
INSERT INTO country (pic,countryname) VALUES ('FM', 'Micronesia, Federated States of');
INSERT INTO country (pic,countryname) VALUES ('MD', 'Moldova, Republic of');
INSERT INTO country (pic,countryname) VALUES ('MC', 'Monaco');
INSERT INTO country (pic,countryname) VALUES ('MN', 'Mongolia');
INSERT INTO country (pic,countryname) VALUES ('ME', 'Montenegro');
INSERT INTO country (pic,countryname) VALUES ('MS', 'Montserrat');
INSERT INTO country (pic,countryname) VALUES ('MA', 'Morocco');
INSERT INTO country (pic,countryname) VALUES ('MZ', 'Mozambique');
INSERT INTO country (pic,countryname) VALUES ('MM', 'Myanmar');
INSERT INTO country (pic,countryname) VALUES ('NA', 'Namibia');
INSERT INTO country (pic,countryname) VALUES ('NR', 'Nauru');
INSERT INTO country (pic,countryname) VALUES ('NP', 'Nepal');
INSERT INTO country (pic,countryname) VALUES ('NL', 'Netherlands');
INSERT INTO country (pic,countryname) VALUES ('AN', 'Netherlands Antilles');
INSERT INTO country (pic,countryname) VALUES ('NC', 'New Caledonia');
INSERT INTO country (pic,countryname) VALUES ('NZ', 'New Zealand');
INSERT INTO country (pic,countryname) VALUES ('NI', 'Nicaragua');
INSERT INTO country (pic,countryname) VALUES ('NE', 'Niger');
INSERT INTO country (pic,countryname) VALUES ('NG', 'Nigeria');
INSERT INTO country (pic,countryname) VALUES ('NU', 'Niue');
INSERT INTO country (pic,countryname) VALUES ('NF', 'Norfolk Island');
INSERT INTO country (pic,countryname) VALUES ('MP', 'Northern Mariana Islands');
INSERT INTO country (pic,countryname) VALUES ('NO', 'Norway');
INSERT INTO country (pic,countryname) VALUES ('OM', 'Oman');
INSERT INTO country (pic,countryname) VALUES ('PK', 'Pakistan');
INSERT INTO country (pic,countryname) VALUES ('PW', 'Palau');
INSERT INTO country (pic,countryname) VALUES ('PS', 'Palestine');
INSERT INTO country (pic,countryname) VALUES ('PA', 'Panama');
INSERT INTO country (pic,countryname) VALUES ('PG', 'Papua New Guinea');
INSERT INTO country (pic,countryname) VALUES ('PY', 'Paraguay');
INSERT INTO country (pic,countryname) VALUES ('PE', 'Peru');
INSERT INTO country (pic,countryname) VALUES ('PH', 'Philippines');
INSERT INTO country (pic,countryname) VALUES ('PN', 'Pitcairn');
INSERT INTO country (pic,countryname) VALUES ('PL', 'Poland');
INSERT INTO country (pic,countryname) VALUES ('PT', 'Portugal');
INSERT INTO country (pic,countryname) VALUES ('PR', 'Puerto Rico');
INSERT INTO country (pic,countryname) VALUES ('QA', 'Qatar');
INSERT INTO country (pic,countryname) VALUES ('RE', 'Reunion');
INSERT INTO country (pic,countryname) VALUES ('RO', 'Romania');
INSERT INTO country (pic,countryname) VALUES ('RU', 'Russian Federation');
INSERT INTO country (pic,countryname) VALUES ('RW', 'Rwanda');
INSERT INTO country (pic,countryname) VALUES ('KN', 'Saint Kitts and Nevis');
INSERT INTO country (pic,countryname) VALUES ('LC', 'Saint Lucia');
INSERT INTO country (pic,countryname) VALUES ('VC', 'Saint Vincent and the Grenadines');
INSERT INTO country (pic,countryname) VALUES ('WS', 'Samoa');
INSERT INTO country (pic,countryname) VALUES ('SM', 'San Marino');
INSERT INTO country (pic,countryname) VALUES ('ST', 'Sao Tome and Principe');
INSERT INTO country (pic,countryname) VALUES ('SA', 'Saudi Arabia');
INSERT INTO country (pic,countryname) VALUES ('SN', 'Senegal');
INSERT INTO country (pic,countryname) VALUES ('RS', 'Serbia');
INSERT INTO country (pic,countryname) VALUES ('SC', 'Seychelles');
INSERT INTO country (pic,countryname) VALUES ('SL', 'Sierra Leone');
INSERT INTO country (pic,countryname) VALUES ('SG', 'Singapore');
INSERT INTO country (pic,countryname) VALUES ('SK', 'Slovakia');
INSERT INTO country (pic,countryname) VALUES ('SI', 'Slovenia');
INSERT INTO country (pic,countryname) VALUES ('SB', 'Solomon Islands');
INSERT INTO country (pic,countryname) VALUES ('SO', 'Somalia');
INSERT INTO country (pic,countryname) VALUES ('ZA', 'South Africa');
INSERT INTO country (pic,countryname) VALUES ('GS', 'South Georgia South Sandwich Islands');
INSERT INTO country (pic,countryname) VALUES ('ES', 'Spain');
INSERT INTO country (pic,countryname) VALUES ('LK', 'Sri Lanka');
INSERT INTO country (pic,countryname) VALUES ('SH', 'St. Helena');
INSERT INTO country (pic,countryname) VALUES ('PM', 'St. Pierre and Miquelon');
INSERT INTO country (pic,countryname) VALUES ('SD', 'Sudan');
INSERT INTO country (pic,countryname) VALUES ('SR', 'Suriname');
INSERT INTO country (pic,countryname) VALUES ('SJ', 'Svalbard and Jan Mayen Islands');
INSERT INTO country (pic,countryname) VALUES ('SZ', 'Swaziland');
INSERT INTO country (pic,countryname) VALUES ('SE', 'Sweden');
INSERT INTO country (pic,countryname) VALUES ('CH', 'Switzerland');
INSERT INTO country (pic,countryname) VALUES ('SY', 'Syrian Arab Republic');
INSERT INTO country (pic,countryname) VALUES ('TW', 'Taiwan');
INSERT INTO country (pic,countryname) VALUES ('TJ', 'Tajikistan');
INSERT INTO country (pic,countryname) VALUES ('TZ', 'Tanzania, United Republic of');
INSERT INTO country (pic,countryname) VALUES ('TH', 'Thailand');
INSERT INTO country (pic,countryname) VALUES ('TG', 'Togo');
INSERT INTO country (pic,countryname) VALUES ('TK', 'Tokelau');
INSERT INTO country (pic,countryname) VALUES ('TO', 'Tonga');
INSERT INTO country (pic,countryname) VALUES ('TT', 'Trinidad and Tobago');
INSERT INTO country (pic,countryname) VALUES ('TN', 'Tunisia');
INSERT INTO country (pic,countryname) VALUES ('TR', 'Turkey');
INSERT INTO country (pic,countryname) VALUES ('TM', 'Turkmenistan');
INSERT INTO country (pic,countryname) VALUES ('TC', 'Turks and Caicos Islands');
INSERT INTO country (pic,countryname) VALUES ('TV', 'Tuvalu');
INSERT INTO country (pic,countryname) VALUES ('UG', 'Uganda');
INSERT INTO country (pic,countryname) VALUES ('UA', 'Ukraine');
INSERT INTO country (pic,countryname) VALUES ('AE', 'United Arab Emirates');
INSERT INTO country (pic,countryname) VALUES ('GB', 'United Kingdom');
INSERT INTO country (pic,countryname) VALUES ('US', 'United States');
INSERT INTO country (pic,countryname) VALUES ('UM', 'United States minor outlying islands');
INSERT INTO country (pic,countryname) VALUES ('UY', 'Uruguay');
INSERT INTO country (pic,countryname) VALUES ('UZ', 'Uzbekistan');
INSERT INTO country (pic,countryname) VALUES ('VU', 'Vanuatu');
INSERT INTO country (pic,countryname) VALUES ('VA', 'Vatican City State');
INSERT INTO country (pic,countryname) VALUES ('VE', 'Venezuela');
INSERT INTO country (pic,countryname) VALUES ('VN', 'Vietnam');
INSERT INTO country (pic,countryname) VALUES ('VG', 'Virgin Islands (British)');
INSERT INTO country (pic,countryname) VALUES ('VI', 'Virgin Islands (U.S.)');
INSERT INTO country (pic,countryname) VALUES ('WF', 'Wallis and Futuna Islands');
INSERT INTO country (pic,countryname) VALUES ('EH', 'Western Sahara');
INSERT INTO country (pic,countryname) VALUES ('YE', 'Yemen');
INSERT INTO country (pic,countryname) VALUES ('ZR', 'Zaire');
INSERT INTO country (pic,countryname) VALUES ('ZM', 'Zambia');
INSERT INTO country (pic,countryname) VALUES ('ZW', 'Zimbabwe');

SET IDENTITY_INSERT [dbo].[Product] ON
INSERT INTO [dbo].[Product] ([productID], [productName], [price], [info], [pic]) VALUES (1, N'Tito''s Handmade Vodka', 27, NULL, N'http://www.totalwine.com/media/sys_master/twmmedia/h4a/hdc/9896147025950.png')
INSERT INTO [dbo].[Product] ([productID], [productName], [price], [info], [pic]) VALUES (2, N'Ketel One', 28, NULL, N'http://www.totalwine.com/media/sys_master/twmmedia/hb5/h6e/8816902832158.png')
INSERT INTO [dbo].[Product] ([productID], [productName], [price], [info], [pic]) VALUES (3, N'Smirnoff', 15, NULL, N'http://www.totalwine.com/media/sys_master/twmmedia/h9f/h61/8810507894814.png')
SET IDENTITY_INSERT [dbo].[Product] OFF

INSERT INTO [dbo].[AlcoholProduct] ([productID], [categoryID], [volume], [origin]) VALUES (1, 6, 40, N'Antigua and Barbuda');
INSERT INTO [dbo].[AlcoholProduct] ([productID], [categoryID], [volume], [origin]) VALUES (2, 6, 40, N'Afghanistan');
INSERT INTO [dbo].[AlcoholProduct] ([productID], [categoryID], [volume], [origin]) VALUES (3, 6, 40, N'Afghanistan');
