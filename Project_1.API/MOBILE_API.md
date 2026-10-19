# تطبيق الجيم

الـAPI يستخدم نفس طبقات المشروع: Controller → Service → UnitOfWork → Repository → AppDbContext.
الـDTOs في Application والمودلز في Domain. صفحات MVC والصلاحيات الحالية مستمرة.

## تشغيل التحديث

بعد Pull، افتح Package Manager Console من مجلد الـSolution ونفّذ:

```powershell
Update-Database -Project Project_1.Infrastructure -StartupProject "Project 1"
```

أضفنا Migration باسم `AddMobileGymFeatures`؛ لا تنشئ Migration ثانية لنفس التغيير.
لا يُشغّل الـAPI تحديث قاعدة البيانات تلقائيًا.

بعدها من الموقع:

1. عدّل الخطط وحدّد الفئة Express أو Premium والمدة الفعلية بالأشهر. الخطط القديمة محفوظة وغير مصنفة؛ لن تظهر في التطبيق حتى تصنّفها. المدة الافتراضية التقنية شهر واحد، لذلك اضبط مدة كل خطة عند تصنيفها.
2. من الوظائف، فعّل «وظيفة مدرب (تظهر في التطبيق)» للوظائف المناسبة. الـAPI يعرض موظفي هذه الوظائف فقط.
3. شغّل Project_1.API وافتح Swagger.

التسجيل من التطبيق ينشئ User وMember مرتبطين في عملية حفظ واحدة، بدون صلاحيات موظفين أو إدارة. كلمة المرور تُحفظ كـHash فقط؛ يبقى عمود Password القديم للتوافق لكنه فارغ للحسابات الجديدة وتغيير كلمة المرور.
حسابات وأعضاء الموقع القدامى لا نربطهم تلقائيًا بالاسم أو الإيميل لتجنّب ربط شخص بالعضو الخطأ. الحساب القديم غير المرتبط يستطيع الدخول، لكن السلة تتطلب ربطه بعضو؛ التسجيل الجديد يجهز الربط تلقائيًا.

## المسارات

| الطلب | المسار | الغرض |
|---|---|---|
| GET | `/api/Categories` | فئتا النادي ومزاياهما |
| GET | `/api/Plans?categoryId=1` | باقات إكسبرس (2 لبريميوم) |
| GET | `/api/Plans/{id}` | تفاصيل باقة مصنفة |
| GET | `/api/Trainers` | المدربون باستخدام EmployeeId |
| POST | `/api/Users/register` | إنشاء الحساب والعضوية |
| POST | `/api/Users/login` | تسجيل الدخول وإنشاء جلسة |
| GET | `/api/Users/profile` | حساب المستخدم الحالي، بدون كلمة المرور أو الـHash |
| PUT | `/api/Users/profile` | تعديل الاسم والإيميل والهاتف |
| GET | `/api/Cart` | `{ "item": null }` أو الخطة المختارة |
| POST | `/api/Cart` | اختيار/استبدال الخطة والمدرب |
| DELETE | `/api/Cart` | إلغاء الاختيار |
| POST | `/api/Cart/confirm` | إنشاء الاشتراك وتفريغ السلة |
| GET | `/api/Subscriptions` | اشتراكات العضو المرتبط بالحساب الحالي |
| POST | `/api/auth/logout` | إنهاء الجلسة |

مسارات العرض العامة للفئات والخطط والمدربين لا تحتاج دخولًا. الملف الشخصي والسلة والاشتراكات تحتاج جلسة دخول.
UserId يؤخذ من جلسة الدخول في السيرفر، ولا يُرسل من Flutter. السلة مخزنة بـUserId + PlanId + EmployeeId؛ الاشتراك يستخدم MemberId + PlanId + EmployeeId.

## الجلسة في Flutter وSwagger

حافظنا على جلسات Cookies وحماية CSRF الموجودة في الـAPI. التطبيق يحتاج HTTP client يحتفظ بالـCookies عبر الطلبات (cookie jar)، وليس http.post مستقلة تفقد الجلسة.

1. `GET /api/auth/csrf`، احفظ Cookie الحماية والتوكن من حقل `token`.
2. أرسل التوكن في Header باسم `X-CSRF-TOKEN` مع كل POST/PUT/DELETE، وأرسل الـCookies المحفوظة.
3. بعد نجاح login، احفظ Cookie الجلسة، ثم اطلب `/api/auth/csrf` مجددًا للحصول على توكن مطابق للجلسة المسجّلة.
4. في Swagger: نفّذ csrf ثم أدخل التوكن في Authorize، وبعد login كرر csrf وحدّث Authorize.

```json
POST /api/Users/register
{
  "name": "Hisham",
  "email": "hisham@example.com",
  "username": "hisham",
  "password": "Example123",
  "phone": "0500000000"
}
```

```json
POST /api/Users/login
{ "username": "hisham", "password": "Example123" }
```

```json
POST /api/Cart
{ "planId": 1, "employeeId": 3 }
```

السلة تحتوي خطة واحدة فقط؛ اختيار خطة جديدة يستبدل السابقة، بدون Quantity. السعر والمدة يؤخذان من قاعدة البيانات. بعد الإضافة اعرض GET /api/Cart للمراجعة، ثم أرسل قيمة `item.version`:

```json
POST /api/Cart/confirm
{ "version": "القيمة التي رجعت من GET /api/Cart" }
```

التأكيد يبدأ الاشتراك الآن، ويحسب النهاية بـAddMonths حسب الخطة. لا يوجد دفع مالي في هذه الخطوة. وجود اشتراك سارٍ أو قادم يمنع إنشاء اشتراك آخر؛ التأكيد المتكرر أو الاختيار القديم لا ينشئ اشتراكًا إضافيًا.

## الاختبارات

من جذر المستودع:

```bash
dotnet run --project Tests/Project_1.Mobile.Tests
```

اختبارات تكامل على SQLite مؤقتة تشمل التسجيل والربط وتجزئة كلمة المرور، الصلاحيات القديمة، اختيار الخطة والمدرب، عزل الحسابات، مدة الاشتراك، وتراجع العملية عند تعارض تعديل السلة. لا تتصل بقاعدة SQL Server الخاصة بالمستخدم. يلزم تطبيق الـMigration وتجربة SQL Server محليًا.

ملفات Flutter الحالية غير موجودة في هذا المستودع؛ هذه الإضافة تجهز الباك اند لشاشات التطبيق السبع المتفق عليها.
