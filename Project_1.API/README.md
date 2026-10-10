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

| المسار | العملية | الصلاحية |
|---|---|---|
| `GET /api/employees/GetEmployees` | `Emp`: قائمة الموظفين بواسطة `GetEmployeesImprove` | Read |
| `POST /api/employees` | `CreateEmployee`: إضافة موظف، والنجاح يرجع `Data Added` | Read + Create |

هذه أسماء الأكشنات والمسارات ومنطق استدعاء السيرفس في كود المدرب المرسل. معامل `name` اختياري ولا يرشّح النتائج؛ كود المدرب لا يستخدمه أيضاً. الإضافة تستقبل `multipart/form-data` لأن DTO يتضمن صورة `IFormFile`، لذلك أضيف `[FromForm]`.

أُبقي تسجيل الدخول والتحقق من الصلاحيات وحماية طلب الإضافة، لتطبيق صلاحيات المشروع. بدون دخول يرجع 401، وبدون الصلاحية يرجع 403. `POST /api/auth/logout` ينهي جلسة API.

الصلاحيات تُقرأ عند تسجيل الدخول؛ بعد تعديل أدوار المستخدم سجّل الخروج والدخول من جديد.

تمت مطابقة EmployeesController وWeatherForecastController مع الكود الذي أرسله المستخدم. مثال الأسماء في `GET /WeatherForecast/names` ومثال الطقس في `GET /WeatherForecast/weather` متاحان دون دخول؛ بياناتهما تجريبية ثابتة/عشوائية ولا يقرآن قاعدة بيانات النادي. تمت مراجعة Program.cs وملف المشروع وWeatherForecast.cs مقابل ملفات المدرب المرسلة. ملف المشروع يستخدم .NET 8 وSwashbuckle 6.6.2 ومرجع Application فقط، مثل المدرب؛ مراجع Infrastructure وDomain تأتي عبر Application. Program.cs يسجل DbContext وUnitOfWork وEmployeeService وControllers وSwagger بالترتيب نفسه، مع إبقاء إعدادات الدخول والصلاحيات وحماية طلب الإضافة وخدمة الصور الخاصة بمشروع النادي.
