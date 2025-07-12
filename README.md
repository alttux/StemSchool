# Tweaker for School

## Описание проекта
**Tweaker for School** – это утилита для оптимизации работы Windows в школьных компьютерных классах. Программа позволяет отключать анимации и прозрачность, управлять прокси-сервером и настраивать сеть ESPD.

---

## Cкриншоты
![image](https://github.com/alttux/StemSchool/blob/master/Screenshots/main_screen.png)
![image](https://github.com/alttux/StemSchool/blob/master/Screenshots/opt.png)
![image](https://github.com/alttux/StemSchool/blob/master/Screenshots/walls.png)
![image](https://github.com/alttux/StemSchool/blob/master/Screenshots/explorer.png)
![image](https://github.com/alttux/StemSchool/blob/master/Screenshots/KMS_MAS.png)

---

## Функционал
- **Отключение/включение анимаций окон** (разворачивание, сворачивание, эффекты)  
- **Отключение/включение прозрачности**  
- **Отключение/включение телеметрии Microsoft**  
- **Настройка прокси-сервера** (ввод адреса и порта)  
- **Настройка школьной сети ESPD**
- **Редактирование групповых политик**
- **Активация Windows и Ofiice**

---

## Технологии
- **C# (.NET)**
- **WPF (Windows Presentation Foundation)**
- **Работа с реестром Windows (Registry)**
- **P/Invoke (SystemParametersInfo, User32.dll)**
- **Работа с сетевыми настройками Windows (ESP и Proxy)**

---

## Установка и запуск
### Запуск без сборки
1. **Скачайте** последний установочный файл из **[Releases](https://github.com/CodeCraftsman89/StemSchool/releases)**
2. Запустите `.exe`

### Сборка из исходного кода
1. **Клонируйте репозиторий**:
   ```sh
   git clone https://github.com/CodeCraftsman89/StemSchool/releases
2. Откройте и скомпилируйте через Visual Studio IDE (Обязательно установленный WPF)


**[Этот проект распространяется под лицензией MIT](https://github.com/CodeCraftsman89/StemSchool/blob/master/LICENSE.txt)**