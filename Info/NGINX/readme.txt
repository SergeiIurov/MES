Создать тестовый сертификат    
openssl req -x509 -nodes -days 365 -newkey rsa:2048 -keyout nginx-selfsigned.key -out nginx-selfsigned.crt

Проверка полученного сертификата
openssl x509 -noout -modulus -in E:\cert\nginx-selfsigned.crt | openssl md5
openssl rsa -noout -modulus -in E:\cert\nginx-selfsigned.key | openssl md5


проверка конфига
nginx -t

nginx - запуск


Перегрузка
nginx -s reload