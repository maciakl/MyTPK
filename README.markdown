My Temporary Public Key
=======================

MyTPK is a simple, public key encryption tool. The aim of this application is to make public key encryption as simple and as painless as possible so that it can be used even by compute illiterate end-users.

MyTPK eschews cumbersome key management schemes by introducing a concept of a temporary public key. Instead of always using the same key pair, and collecting public keys on a key ring, users will simply generate a new key pair for each communication.

MyTPK is not intended to replace more robust schemes such as GPG or PGP. It is designed to be a tool that makes occasional exchange of encrypted documents slightly more secure.

For example, imagine that two users try to exchange confidential Word document over email. Neither of them knows much about encryption. Most common encryption method available to them is the built in AES feature of WinZip. Unfortunately AES is a symmetric cipher, which means the users will need to exchange a password. Chances are they will probably pick an easy one (eg. "kittens") so that it can be easily exchanged over the phone, or send it in another email. In both cases the key exchange is insecure and risky.

MyTPK gives them another option. Both users generate a key pair, exchange public keys over email and then transfer files securely. The GUI is designed to be easy to use and easy to understand, even if the user does not know much about asymmetric encryption schemes.

Requirements
------------

MyTPK requires Microsoft .NET Framework 3.5 or higher. The project was created using Microsoft Visual C# 2008 Express, and the repository includes all the project files for your convenience. You should be able to open and compile the project in any version of Microsoft Visual Studio.

Installation
------------

At the moment MyTPK is a stand-alone, self contained executable. No installation is necessary.


Encryption
----------

The encryption process is as follows:

- Both users generate a 256 bit RSA key pair
- Recipient emails her public key to the sender
- Sender generates a random 32 bit AES key
- Sender encrypts the file with the AES key
- Sender encrypts the AES key and IV with the 256 bit public RSA key of the recipient
- Sender concatenates the message and the encrypted key, then emails the resulting file to the recipient
- Recipient breaks the message apart
- Recipient decrypts the AES key with his private RSA key
- Recipient uses the AES key to decrypt the message

Most of the above is hidden from the user.


Key Storage
-----------

MyTPK stores the asymmetric key pair in the default system key storage in registry. The AES key and IV are stored on the file system usually in %APPDATA%\MyTPK folder.

License
-------

This program is free software; you can redistribute it and/or modify it under the terms of the GNU General Public License as published by the Free Software Foundation; either version 3 of the License, or (at your option) any later version. This program is distributed in the hope that it will be useful, but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU General Public License for more details.

Copyright (c) 2011 Lukasz Grzegorz Maciak (maciak.org)


