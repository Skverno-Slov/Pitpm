%include "io.inc"

section .text
global main
main:
    PRINT_STRING 'enter number: '
    GET_DEC 4, eax
    
    mov edx, eax
    add edx, 1
     eax, edx
    
    PRINT_DEC 4, eax
    xor eax, eax
    ret