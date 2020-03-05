## Регистрация пользователя

> POST /sklexp/regist

```shell
curl -v X POST \
    -H "Content-Type: application/json" \
    -d '{"firstname":"Roman","secondname":"Doronin","username":"rdoronin","hash_password":"#9jfg3ksg34jh34g9$4geeg54zm"}' \
{
    "status": "success",
    "message": "ok"
}
```
# 200 Все хорошо
# 409 Уже существует такой username

----------------------------------------------------

## Авторизация пользователя

> PUT /sklexp/auth

```shell
curl -v X PUT \
    -H "Content-Type: application/json" \
    -d '{"username":"rdoronin","hash_password":"#9jfg3ksg34jh34g9$4geeg54zm"}' \
{
    "data": {
        "account_id": "kg9254p94q3p43cf4cfp4fjq38"
    }
    "status": "success",
    "message": "ok"
}
```
# 200 Все хорошо
# 401 Неправильный username или пароль

----------------------------------------------------

## Добавление пациента

> POST /sklexp/accounts/{ACCOUNT_ID}/patient

```shell
curl -v X POST \
    -H "Content-Type: application/json" \
    -d '{"firstname":"Patient67","secondname":"Ivanov"}' \
{
    "status": "success",
    "message": "ok"
}
```
# 200 Все хорошо


## Запрос на всех пациентов

> GET /sklexp/accounts/{ACCOUNT_ID}/patient

```shell
curl -v X GET \
    -H "Content-Type: application/json" \
{
    "data": {
        "patients": [
            {
                "patient_id": "{PATIENT_ID}",
                "firstname":"Patient67",
                "secondname":"Ivanov",
                "pet_id":"{PET_ID}"
             },
             {
                "patient_id": "{PATIENT_ID}",
                "firstname":"Patient68",
                "secondname":"Ivanov",
                "pet_id":"{PET_ID}"
             }
        ]
    }
    "status": "success",
    "message": "ok"
}
```
# 200 Все хорошо


## Запросить данные пациента

> GET /sklexp/accounts/{ACCOUNT_ID}/patient/{PATIENT_ID}

```shell
curl -v X GET \
    -H "Content-Type: application/json" \
{
    "data": {
        "firstname": "Patient67",
        "secondname": "Ivanov",
        "pet_id": "{PET_ID}"
    }
    "status": "success",
    "message": "ok"
}
```
# 200 Все хорошо
# 404 Пациент не найден


## Добавление у пациента медецинской карты животного

> PATCH /sklexp/accounts/{ACCOUNT_ID}/patient/{PATIENT_ID}

```shell
curl -v X PATCH \
    -H "Content-Type: application/json" \
    -d '{"pet_medical_card_id":"{PET_MEDICAL_CARD_ID}"}' \
{
    "status": "success",
    "message": "ok"
}
```
# 200 Все хорошо
# 404 Пациент не найден / Медецинская карта животного не найдена

----------------------------------------------------

## Добавление медецинской карты животного

> POST /sklexp/accounts/{ACCOUNT_ID}/petmc

```shell
curl -v X POST \
    -H "Content-Type: application/json" \
    -d '{"name":"Salem","disease":"Cold"}' \
{
    "status": "success",
    "message": "ok"
}
```
# 200 Все хорошо


----------------------------------------------------

## Запросить даннее медицинской карты животного

> GET /sklexp/accounts/{ACCOUNT_ID}/petmc/{PET_MEDICAL_CARD_ID}

```shell
curl -v X GET \
    -H "Content-Type: application/json" \
{
    "data": {
        "name": "Salem",
        "disease": "Cold"
    }
    "status": "success",
    "message": "ok"
}
```
# 200 Все хорошо
# 404 Медецинская карта животного не найдена
