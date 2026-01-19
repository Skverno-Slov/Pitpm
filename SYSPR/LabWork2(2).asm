%include "io.inc"
    
section .text

global main
main:
    PRINT_STRING 'y(x)=ax2+bx+c'
    NEWLINE
    PRINT_STRING 'a: '
    GET_DEC 4, eax
    PRINT_STRING 'x: '
    GET_DEC 4, ebx
    PRINT_STRING 'b: '
    GET_DEC 4, edx
    PRINT_STRING 'c: '
    GET_DEC 4, ecx
    
    imul edx, ebx
    add ecx, edx
    imul ebx
    imul eax, ebx
    add eax, ecx
    
    PRINT_STRING 'y = '
    PRINT_DEC 4, eax  
    xor eax, eax
    xor ebx, ebx
    xor edx, edx
    xor ecx, ecx
    ret