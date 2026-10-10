# Project_1.API

مشروع ASP.NET Core Web API مستقل على .NET 8، يستخدم Application وInfrastructure وDomain الموجودة في الحل. لا يكرر منطق CRUD داخل Controllers.

## التشغيل

1. عدّل `ConnectionStrings:DefaultDatabase` في `appsettings.json` لقاعدة البيانات نفسها التي يستخدمها MVC.
2. في Visual Studio اضغط بزر الفأرة الأيمن على `Project_1.API` واختر **Set as Startup Project**، ثم شغّل ملف `https`.
3. افتح `https://localhost:7250/swagger` في وضع Development.

يبقى `Project 1` هو مشروع MVC؛ اختره عند تشغيل واجهة النادي.

## تسجيل الدخول في Swagger

الـAPI يستخدم جلسة مستقلة عن MVC، وحسابات المستخدمين وكلمات المرور والصلاحيات نفسها في قاعدة البيانات.

1. نفّذ `GET /api/auth/csrf` وانسخ `token` من الرد.
2. اضغط **Authorize** والصق القيمة في حقل CSRF.
3. نفّذ `POST /api/auth/login` مع `username` و`password`. يحتفظ المتصفح بملف الجلسة تلقائياً.
4. نفّذ `GET /api/auth/csrf` مرة أخرى بعد تسجيل الدخول وحدّث القيمة في **Authorize**.
5. استخدم المسارات التالية. مع Postman احتفظ بملفات cookies وأرسل الرمز في ترويسة `X-CSRF-TOKEN` عند POST وPUT وDELETE.

| المسار | GET | POST | PUT /{id} | DELETE /{id} |
|---|---|---|---|---|
| `/api/employees` | قائمة أو عنصر بواسطة /{id} | موظف جديد (multipart/form-data) | تعديل موظف (multipart/form-data) | حذف |
| `/api/jobs` | قائمة أو عنصر بواسطة /{id} | وظيفة جديدة (JSON) | تعديل وظيفة (JSON) | حذف |
| `/api/departments` | قائمة أو عنصر بواسطة /{id} | قسم جديد (JSON) | تعديل قسم (JSON) | حذف |

كل العمليات تتطلب Read. الإضافة تتطلب Create، والتعديل Edit، والحذف Delete. بدون دخول يرجع 401، وبدون الصلاحية يرجع 403. `POST /api/auth/logout` ينهي جلسة API.

الصلاحيات تُقرأ عند تسجيل الدخول؛ بعد تعديل أدوار المستخدم سجّل الخروج والدخول من جديد.

تعذّر الوصول إلى رابط مستودع المدرب أثناء الإضافة؛ هذا المشروع يطبّق تقسيم الطبقات المعروض، ولا يدّعي مطابقة مسارات نسخته حرفياً.
