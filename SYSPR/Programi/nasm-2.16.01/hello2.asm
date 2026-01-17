global _start       ; делаем метку метку _start видимой извне
 
extern WriteFile        ; подключем функцию WriteFile
extern GetStdHandle     ; подключем функцию GetStdHandle
 
section .data   ; секция данных
message: db "Hello World!",10  ; строка для вывода на консоль
 
section .text       ; объявление секции кода
_start:             ; метка _start - точка входа в программу
    sub  rsp, 40   ; Для параметров функций WriteFile и GetStdHandle резервируем 40 байт (5 параметров по 8 байт)
    mov  rcx, -11  ; Аргумент для GetStdHandle - STD_OUTPUT
    call GetStdHandle ; вызываем функцию GetStdHandle
    mov  rcx, rax     ; Первый параметр WriteFile - в регистр RCX помещаем дескриптор файла - консоли
    mov  rdx, message    ; Второй параметр WriteFile - загружаем указатель на строку в регистр RDX
    mov  r8d, 18      ; Третий параметр WriteFile - длина строки для записи в регистре R8D 
    xor  r9, r9       ; Четвертый параметр WriteFile - адрес для получения записанных байтов
    mov  qword [rsp + 32], 0  ; Пятый параметр WriteFile
    call WriteFile ; вызываем функцию WriteFile
    add  rsp, 40
    ret             ; выход из программы
