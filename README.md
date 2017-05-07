# Crypto

A simple library for encryption and decryption.

## Supports

Includes the support of the following cryptosystems:
- Caesar cipher;
- Simple substitution cipher;
- Visenere cipher.

## Customisation

The library is highly customisable.

It can work with any alphabet. The following alphabets are included out-of-the-box:
- English (uppercase);
- English (lowercase);
- English (both upper and lowercase);
- Ukrainian (uppercase);
- Ukrainian (lowercase);
- Ukrainian (both upper and lowercase);
- Punctuation ( .,;:!?/\'"-()`);
- Special characters ([]{}<>~@#$%^&*-_+=);
- English with punctuation;
- Ukrainian with punctuation.

The alphabets can be combined with each other or any other custom alphabet.

All cryptosystems have a strict mode where they throw exceptions
if the message does not belong to the system's alphabet.
If the strict mode is off, any unknown characters will stay the same
during encryption or decryption.
