## Регистрация пользователя

> POST /sklexp/regist

```shell
curl -v X POST \
    -H "Content-Type: application/json" \
    -d '{"firstname":"Roman","secondname":"Doronin","username":"rdoronin","hash_password":"#9jfg3ksg34jh34g9$4geeg54zm"}' \
{
    "message": "success"
}
```
```shell
409 Conflict - Уже существует такой логин
```

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
    "message": "success"
}
```
```shell
401 Unauthorized - Неправильный логин или пароль
```

----------------------------------------------------

## Добавление пациента

> POST /sklexp/accounts/{ACCOUNT_ID}/patient

```shell
curl -v X POST \
    -H "Content-Type: application/json" \
    -d '{"firstname":"Patient67","secondname":"Ivanov"}' \
{
    "message": "success"
}
```

## Получение всех пациентов

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
    "message": "success"
}
```

## Получение конкретного пациента

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
    "message": "success"
}
```
```shell
404 Not Found - Пациент с таким {PATIENT_ID} не найден
```

## Добавление у пациента медецинской карты животного

> PATCH /sklexp/accounts/{ACCOUNT_ID}/patient/{PATIENT_ID}

```shell
curl -v X PATCH \
    -H "Content-Type: application/json" \
    -d '{"pet_medical_card_id":"{PET_MEDICAL_CARD_ID}"}' \
{
    "message": "success"
}
```
```shell
404 Not Found - Пациент с таким {PATIENT_ID} не найден
404 Not Found - Медецинская карта животного с таким {PET_MEDICAL_CARD_ID} не найдена
```

----------------------------------------------------

## Добавление медецинской карты животного

> POST /sklexp/accounts/{ACCOUNT_ID}/petmc

```shell
curl -v X POST \
    -H "Content-Type: application/json" \
    -d '{"name":"Salem","disease":"Cold"}' \
{
    "message": "success"
}
```

## Получение конкретной медицинской карты животного

> GET /sklexp/accounts/{ACCOUNT_ID}/petmc/{PET_MEDICAL_CARD_ID}

```shell
curl -v X GET \
    -H "Content-Type: application/json" \
{
    "data": {
        "name": "Salem",
        "disease": "Cold"
    }
    "message": "success"
}
```
```shell
404 Not Found - Медецинская карта животного с таким {PET_MEDICAL_CARD_ID} не найдена
```
