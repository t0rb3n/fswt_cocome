import os
import random
import time
import datetime

FILENAME = 'db_content.sql'
PRUCHASE_PRICE_MIN = 2.00
PRUCHASE_PRICE_MAX = 30.00
STOCK_AMOUNT_MIN = 1
STOCK_AMOUNT_MAX = 40
STOCK_MIN = 10
STOCK_MAX = 50
ORDER_AMOUNT_MIN = 5
ORDER_AMOUNT_MAX = 15

enterprise_names = ['CocomeSystem GmbH & Co. KG']
store_name = ['Cocome WI', 'Cocome F', 'Cocome MZ']
store_locations = ['Wiesbaden', 'Frankfurt', 'Mainz']
supplier_names = ['Schegel', 'Kaufmann', 'Lutz GmbH', 'Scheffler GmbH', 'Thiele', 'Betz Lange']
product_names = ['Feldsalat', 'Fleischwurst', 'Ferrero Nutella Schokocreme', 'Storck Knoppers', 'Knorr Suppenliebe', 'Milka Alpen Tafelschokolade'
                ,'Head & Shoulders Shampoo', 'Blend-a-med Zahncreme', 'Alpenhain Back-Käse', 'Funny-frisch Chipsfrisch', 'Haribo Fruchtgummi'
                ,'Weißer Rise', 'Coca Cola', 'Dr. Oetker Ristorante Pizza', 'Zott Sahnejoghurt', 'Bio-Schweineschnitzel', 'Bio-Entrecôte'
                ,'Clementinen', 'KÖLLN Müsli', 'Perwoll Waschmittel', 'Jagdwurst', 'Toilettenpapier', 'VARTA Batterien', 'Apfelsaft', 'SELTERS Mineralwasser'
                ,'Rotkohl', 'Endiviensalat', 'Paprika', 'Bio-Tafeläpfel', 'Rinderhackfleisch', 'Wolfsbarsch', 'Puten-Oberkeule', 'Weizenbrot', 'Hot Dog'
                ,'HEISSE TASSE Instant-Suppe', 'Martini', 'HENKELL Sekt', 'Deutsche Kartoffeln', 'TEEKANNE Tee Waldbeere', 'Pringles', 'ZOTT Monte Schoko']
productStockitems = list()
supplierProducts = list()

random.seed(322)

def init_file():
    if(os.path.exists(FILENAME)):
        os.remove(FILENAME)
        with open(FILENAME, 'x') as f:
            f.write('-- Dummy data\n')

def writeline_to_file(string:str):
    with open(FILENAME, 'a') as f:
        f.write(string)
        f.write('\n')

def insert_enterprises():
    writeline_to_file('\n-- Enterprise values')

    for enterprise in enterprise_names:
        writeline_to_file(f"INSERT INTO enterprises (name) VALUES ('{enterprise}');")
        print('Creates Enterprise', enterprise)

def insert_supplier():
    writeline_to_file('\n-- ProductSupplier values')

    for supplier in supplier_names:
        writeline_to_file(f"INSERT INTO product_suppliers (name, enterprise_id) VALUES ('{supplier}', 1);")
        print('Creates Supplier', supplier)

def insert_products():
    writeline_to_file('\n-- Product values')
    product_len = len(product_names)
    barcode = 10000000
    product_id = 0

    for supplier in supplier_names:

        sample_len = random.randint(1, product_len)

        for name in random.sample(product_names, sample_len):

            price = round(random.uniform(PRUCHASE_PRICE_MIN, PRUCHASE_PRICE_MAX), 2)
            supplier_id = supplier_names.index(supplier) + 1

            writeline_to_file(f"INSERT INTO products (barcode, purchase_price, name, product_supplier_id) VALUES ({barcode}, {price}, '{name}', {supplier_id});")
            
            barcode += 1
            productStockitems.append([barcode, price])

            product_id += 1
            supplierProducts.append([supplier_id, product_id])

        print('Supplier', supplier, 'were added', sample_len, 'product dummy samples')

def insert_stores():
    writeline_to_file('\n-- Store values')

    for store in zip(store_name, store_locations):
        writeline_to_file(f"INSERT INTO stores (name, location, enterprise_id) VALUES ('{store[0]}', '{store[1]}', 1);")
        print("Creates Store", store)
    
def insert_stock_items():
    writeline_to_file('\n-- StockItem values')
    
    for store_id in range(1, len(store_locations) + 1):
        sample_len = random.randint(1, len(productStockitems) + 1)
        for barcode, price in random.sample(productStockitems, sample_len):
            sale_price = round(price + (price*0.4), 2)
            amount = random.randint(STOCK_AMOUNT_MIN, STOCK_AMOUNT_MAX)
            product_id = productStockitems.index([barcode, price]) + 1
            writeline_to_file(f"INSERT INTO stock_items (sales_price, amount, min_stock, max_stock, store_id, product_id) VALUES ({sale_price}, {amount}, {STOCK_MIN}, {STOCK_MAX}, {store_id}, {product_id});")
        
        print('Store', store_id, 'were inserted', sample_len, 'stock item dummy samples')

def str_time_prop(start, end, time_format, prop):
    stime = time.mktime(time.strptime(start, time_format))
    etime = time.mktime(time.strptime(end, time_format))
    ptime = stime + prop * (etime - stime)
    return time.strftime(time_format, time.localtime(ptime))
 
def get_random_datetime(start, end, prop):
    return str_time_prop(start, end, '%Y-%m-%d %H:%M:%S', prop)

def insert_product_orders():
    writeline_to_file('\n-- ProductOrder values')

    for store_id in range(1, len(store_locations) + 1):
        for x in range(0,3):
            ordering_date = get_random_datetime("2022-02-01 00:00:00", "2022-02-28 23:59:00", random.uniform(0, 0.9))
            if (random.random() > 0.3):  
                delivery_date = get_random_datetime(ordering_date, "2022-02-28 23:59:00", random.random())
                writeline_to_file(f"INSERT INTO product_orders (ordering_date, delivery_date, store_id) VALUES ('{ordering_date}-00', '{delivery_date}-00', {store_id});")
            else:
                writeline_to_file(f"INSERT INTO product_orders (ordering_date, delivery_date, store_id) VALUES ('{ordering_date}-00', '-infinity', {store_id});")

def insert_order_entries():
    writeline_to_file('\n-- OrderEntry values')

    for product_order_id in range(1,10):
        for name in random.sample(supplier_names, 1):

            supplier_id = supplier_names.index(name) + 1
            entries = [oe for oe in supplierProducts if supplier_id == oe[0]]
            sample_len = random.randint(1, len(entries))
        
            for product in random.sample(entries, sample_len):
                amount = random.randint(ORDER_AMOUNT_MIN, ORDER_AMOUNT_MAX)
                writeline_to_file(f"INSERT INTO order_entries (amount, product_id, product_order_id) VALUES ({amount}, {product[1]}, {product_order_id});")

            print('ProductOrder', product_order_id, 'contains', sample_len, 'products from', name)

init_file()
writeline_to_file('START TRANSACTION;')
insert_enterprises()
insert_stores()
insert_supplier()
insert_products()
insert_stock_items()
insert_product_orders()
insert_order_entries()
writeline_to_file('\nCOMMIT;')

