# KUSYS-Demo

Local Db Üzerine kurulmuştur.
MVC Front-End Patternine,Generic Repository ile servisler yazılarak veri tabanı - front end bağlantısı sağlanmıştır.

Seed Data Build aldığında kontrol eder.
Ne yazık ki UnitTesti biliyor olmama rağmen zaman dolayısıyla yetiştiremedim.
Authentication, Authorization işlemleri Microsoft.Identity ile hazırlandı.
DbContextfactory,Serviceprovider gibi farklı yapılar içermektedir.
N-N relation bağlantısına sahiptir.
CourseID CostumizeID'e sahiptir. PK olmasına rağmen istendiği kadar manüple edilebilir.

-------------------------------- KURULUM ----------------------------------------------

Seed Data içerisinde 2 adet rol build aldığında tanımlanır, [Admin,Student]
SEED KULLANICI BİLGİLERİ:
Kullanıcı adı : admin@test.com
Şifre         : 123123

Local db connection stringi değiştirilerek System Administrator kimliği ile giriş yapılabilir..

-----------------------------  EKSİKLER ----------------------------------------------

CRUDLAR'ın öğrenci/kurs bağlantısındaki CRUD işlemleri yarım kaldı.
UnityTest hazırlanamamıştır...
