import os
import random

FILENAME = 'db_content.sql'
PRUCHASE_PRICE_MIN = 2.00
PRUCHASE_PRICE_MAX = 30.00
STOCK_AMOUNT_MIN = 1
STOCK_AMOUNT_MAX = 40
STOCK_MIN = 10
STOCK_MAX = 50

enterprise_names = ['CocomeSystem GmbH & Co. KG']
store_name = ['Cocome']
store_locations = ['Wiesbaden']
supplier_names = ['Schegel', 'Kaufmann', 'Lutz GmbH', 'Scheffler Gmbh', 'Thiele', 'Betz Lange']
product_names = ['Feldsalat', 'Fleischwurst', 'Ferrero Nutella Schokocreme', 'Storck Knoppers', 'Knorr Suppenliebe', 'Milka Alpen Tafelschokolade'
                ,'Head & Shoulders Shampoo', 'Blend-a-med Zahncreme', 'Alpenhain Back-Käse', 'Funny-frisch Chipsfrisch', 'Haribo Fruchtgummi'
                ,'Weißer Rise', 'Coca Cola', 'Dr. Oetker Ristorante Pizza', 'Zott Sahnejoghurt', 'Bio-Schweineschnitzel', 'Bio-Entrecôte'
                ,'Clementinen', 'KÖLLN Müsli', 'Perwoll Waschmittel', 'Jagdwurst', 'Toilettenpapier', 'VARTA Batterien', 'Apfelsaft', 'SELTERS Mineralwasser'
                ,'Rotkohl', 'Endiviensalat', 'Paprika', 'Bio-Tafeläpfel', 'Rinderhackfleisch', 'Wolfsbarsch', 'Puten-Oberkeule', 'Weizenbrot', 'Hot Dog'
                ,'HEISSE TASSE Instant-Suppe', 'Martini', 'HENKELL Sekt', 'Deutsche Kartoffeln', 'TEEKANNE Tee Waldbeere', 'Pringles', 'ZOTT Monte Schoko']
products = list()

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

def insert_supplier():
    writeline_to_file('\n-- ProductSupplier values')

    for supplier in supplier_names:
        writeline_to_file(f"INSERT INTO product_suppliers (name, enterprise_id) VALUES ('{supplier}', 1);")

def insert_products():
    writeline_to_file('\n-- Product values')
    product_len = len(product_names)
    barcode = 10000000

    for supplier in supplier_names:

        sample_len = random.randint(1, product_len)

        for name in random.sample(product_names, sample_len):

            price = round(random.uniform(PRUCHASE_PRICE_MIN, PRUCHASE_PRICE_MAX), 2)
            supplier_id = supplier_names.index(supplier) + 1

            writeline_to_file(f"INSERT INTO products (barcode, purchase_price, name, product_supplier_id) VALUES ({barcode}, {price}, '{name}', {supplier_id});")
            
            barcode += 1
            products.append([barcode, price])

def insert_stores():
    writeline_to_file('\n-- Store values')
    for store in store_name:
        for loc in store_locations:
            writeline_to_file(f"INSERT INTO stores (name, location, enterprise_id) VALUES ('{store}', '{loc}', 1);")

def insert_stock_items():
    writeline_to_file('\n-- StockItem values')
    sample_len = random.randint(1, len(products))

    for barcode, price in random.sample(products, sample_len):
        sale_price = round(price + (price*0.4), 2)
        amount = random.randint(STOCK_AMOUNT_MIN, STOCK_AMOUNT_MAX)
        store_id = random.randint(1, len(store_locations))
        product_id = products.index([barcode, price]) + 1
        writeline_to_file(f"INSERT INTO stock_items (sales_price, amount, min_stock, max_stock, store_id, product_id) VALUES ({sale_price}, {amount}, {STOCK_MIN}, {STOCK_MAX}, {store_id}, {product_id});")


init_file()
writeline_to_file('START TRANSACTION;')
insert_enterprises()
insert_stores()
insert_supplier()
insert_products()
insert_stock_items()
writeline_to_file('\nCOMMIT;')

