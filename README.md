# ElGamal Encryption with Open Key

This project implements a GUI-based ElGamal encryption and decryption system for files with arbitrary content, incorporating advanced cryptographic techniques:
- Fast Modular Exponentiation: Used for efficient encryption/decryption.
- Primitive Root Calculation: Computes the public key ( g ) for a given prime ( p ) using an algorithm to find primitive roots modulo ( p ), with all roots displayed for user selection.
- Parameter Validation: Ensures user-provided parameters (( p ), ( x ), ( k )) meet algorithmic constraints.
- Primality Testing: Employs either Fermat’s or Miller-Rabin probabilistic tests for long arithmetic operations. The program outputs the encrypted file’s contents as decimal numbers, displays the computed ( g ), and generates encrypted/decrypted files, offering a comprehensive tool for public-key cryptography.
---
- Executable file - `TI_Lab3.exe`(or `..\TI_Lab3\TI3\bin\Debug\TI_Lab3.exe`).
- Test files are on the path `Тестовые_файлы`.


![](https://komarev.com/ghpvc/?username=Elizavett-a)
