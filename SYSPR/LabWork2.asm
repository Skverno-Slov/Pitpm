%include "io.inc"

section .text
global main
main:
    PRINT_STRING 'divided: '
    GET_DEC 4, eax      
    PRINT_STRING 'divider: '
    GET_DEC 4, ebx      

    xor edx, edx
    cdq
    idiv ebx             

    PRINT_STRING 'result: '
    PRINT_DEC 4, eax     
    PRINT_STRING ','
    PRINT_DEC 4, edx  

    xor eax, eax
    ret