%include "io.inc"

section .data
    number db 0
    number1 db 0, 3, 2, 3
    x dd 15
    y dd 10
    msg db 'input x: ', 0
    
section .text
global main
main:
    PRINT_STRING msg
    GET_DEC 4, x
    PRINT_STRING 'Input y: '
    GET_DEC 4, y
    
    mov eax, [number]
    movsx eax, byte [number+1] ;смещение на 1 байт (2ой элемент)
    mov ebx, [x]
    cmp ebx, [y]
        ja above
        mov eax, [y]
        jmp exit
    above:
        mov eax, [x]
    exit:
    PRINT_STRING 'min: '
    NEWLINE
    PRINT_DEC 4, eax
    xor eax, eax
    ret