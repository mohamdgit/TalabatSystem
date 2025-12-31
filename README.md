# ECommerce API 

هذا المشروع عبارة عن **eCommerce API** مبني باستخدام **Onion Architecture**.

---

## 📁 بنية المشروع (Clean / Onion Architecture)

```
ECommerceApiSolution/
├── Core/
│   ├── ECommerce.Abstraction/     # الواجهات والعقود Contracts
│   ├── ECommerce.Domain/          # الكيانات Entities والمنطق الأساسي
│   └── ECommerce.Service/         # الخدمات والخدمات المشتركة
│
├── Infrastructure/
│   ├── ECommerce.Persistence/     # الاتصال بقاعدة البيانات + EF Core
│   └── ECommerce.Presentation/    # الطبقة التي تربط الـ API مع الـ Core
│
├── ECommerce.Shared/              # الأكواد المشتركة بين الطبقات
└── ECommerce.Web/                 # طبقة الـ API (Controllers, Endpoints)

```

## 🛠️ التقنيات المستخدمة

* Onion Architecture
* Entity Framework Core
* SQL Server
* Redis 
* JWT Authentication

## 🔐 Authentication

* استخدام JWT Authentication


---

##  تشغيل المشروع   

### 1️⃣ المتطلبات الأساسية
- .NET 9
- SQL Server 
- Visual Studio

### 2️⃣ خطوات التشغيل
1. قم بعمل **Extract** للملف المضغوط.
2. افتح المشروع باستخدام Visual Studio 
3. في مجلد **Infrastructure**، قم بتعديل الاتصال بقاعدة البيانات داخل ملف `appsettings.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=.;Database=ECommerceDb;Trusted_Connection=True;MultipleActiveResultSets=true"
}
````

## 📌 قائمة الـ Endpoints

> يمكنك تنزيل ملف **Postman Collection** الخاص بالـ API :

[📥 ](https://github.com/Qabbaniii/EcommerceAPI-ASP.NETCore/blob/main/ECommerceAPIs.postman_collection.json)[**Download Postman Collection**](https://github.com/Qabbaniii/EcommerceAPI-ASP.NETCore/blob/main/ECommerceAPIs.postman_collection.json)
** 

---

## 🔐 **Authentication**

* **POST** `/api/Authentication/Login` — تسجيل الدخول
* **POST** `/api/Authentication/Register` — تسجيل حساب جديد
* **GET** `/api/Authentication/CheckEmail` — التحقق من وجود بريد إلكتروني
* **GET** `/api/Authentication/CurrentUser` — جلب بيانات المستخدم الحالي
* **GET** `/api/Authentication/Address` — جلب عنوان المستخدم
* **PUT** `/api/Authentication/Address` — تحديث عنوان المستخدم

---

## 🧺 **Basket (Redis)**

* **GET** `/api/Basket` — جلب سلة المستخدم
* **POST** `/api/Basket` — تحديث/إضافة عناصر للسلة
* **DELETE** `/api/Basket` — حذف السلة بالكامل

---

## 📦 **Order**

* **POST** `/api/Order` — إنشاء طلب
* **GET** `/api/Order` — جلب طلبات المستخدم
* **GET** `/api/Order/DeliveryMethods` — جلب طرق التوصيل المتاحة
* **GET** `/api/Order/AllOrders` — جلب كل الطلبات (للمشرفين)

---

## 💳 **Payment**

* **POST** `/api/Payment/{basketId}` — معالجة الدفع لسلة معينة

---

## 🛒 **Product**

* **GET** `/api/Product` — جلب جميع المنتجات
* **GET** `/api/Product/Brands` — جلب العلامات التجارية
* **GET** `/api/Product/Types` — جلب أنواع المنتجات
* **GET** `/api/Product/{id}` — جلب منتج واحد بالتفاصيل
