# Cifrado-DPAPI

**Aplicación de escritorio WPF** para cifrar y descifrar información de forma segura utilizando la **API de Protección de Datos de Windows (DPAPI)**, con soporte de entropía adicional y selección de scope de protección.

---

## 📋 Descripción

**Cifrado-DPAPI** es una herramienta de escritorio para Windows que permite proteger textos sensibles (contraseñas, tokens, cadenas de conexión, etc.) usando el mecanismo nativo de Windows `ProtectedData`, sin necesidad de gestionar claves de cifrado manualmente.

DPAPI vincula el cifrado al perfil del usuario o de la máquina, lo que garantiza que **solo el usuario u equipo autorizado pueda descifrar la información**.

---

## ✨ Características

- 🔒 **Cifrado con DPAPI** — Usa `ProtectedData.Protect` / `Unprotect` de `System.Security.Cryptography`.
- 🔑 **Entropía configurable** — Añade un factor de entropía adicional como segunda capa de protección.
- 👤 **Dos modos de scope**:
  - `Usuario Actual` — Solo el usuario que cifró puede descifrar (por defecto).
  - `Máquina` — Cualquier usuario del mismo equipo puede descifrar.
- 📋 **Salida en Base64** — El texto cifrado se codifica en Base64 para facilitar su almacenamiento y transporte.
- 🛡️ **Manejo seguro de errores** — Método `DescifrarSeguro` que retorna vacío en lugar de lanzar excepciones.

---

## 🏗️ Arquitectura del Proyecto

```
Cifrado_DPAPI/
│
├── GenerarClaves/                  # Proyecto principal WPF
│   ├── DpapiHelper.cs              # Clase estática con la lógica de cifrado/descifrado
│   ├── MainWindow.xaml             # Interfaz de usuario principal
│   ├── MainWindow.xaml.cs          # Code-behind de la ventana principal
│   ├── MainWindowViewModel.cs      # ViewModel (patrón MVVM)
│   ├── App.xaml / App.xaml.cs      # Punto de entrada de la aplicación
│   └── Converts/                   # Convertidores de valores WPF
│
├── GenerarClaves.sln               # Solución de Visual Studio
├── LICENSE                         # Licencia del proyecto
└── README.md                       # Este archivo
```

---

## 🔧 Tecnologías Utilizadas

| Componente | Detalle |
|---|---|
| **Framework** | .NET 9 (`net9.0-windows`) |
| **UI** | WPF (Windows Presentation Foundation) |
| **Patrón** | MVVM con CommunityToolkit.Mvvm |
| **Cifrado** | `System.Security.Cryptography.ProtectedData` (DPAPI) |
| **NuGet** | `CommunityToolkit.Mvvm` 8.4.0 |

---

## 🚀 Requisitos Previos

- **Sistema Operativo:** Windows (el DPAPI es exclusivo de Windows)
- **Runtime:** .NET 9 o superior
- **IDE (opcional):** Visual Studio 2022 o posterior

---

## ▶️ Compilación y Ejecución

1. Clonar el repositorio:

   ```bash
   git clone https://github.com/TuUsuario/Cifrado-DPAPI.git
   ```

2. Abrir la solución en Visual Studio:

   ```
   GenerarClaves.sln
   ```

3. Restaurar los paquetes NuGet y compilar en modo `Release`.

4. Ejecutar el proyecto `GenerarClaves`.

---

## 🧪 Cómo Usar la Aplicación

1. **Ingrese el texto** que desea cifrar en el campo de entrada.
2. **Configure la entropía** (valor por defecto: `0102030405`).  
   > ⚠️ La entropía debe ser **exactamente la misma** al cifrar y al descifrar.
3. **Seleccione el scope** de protección:
   - `Usuario Actual`: Solo el usuario actual puede descifrar.
   - `Máquina`: Cualquier usuario del equipo puede descifrar.
4. Presione **Cifrar** o **Descifrar** según corresponda.
5. El resultado aparecerá en el campo de salida en formato **Base64**.

---

## 🔐 API Pública — `DpapiHelper`

La clase `DpapiHelper` es la pieza central del proyecto y puede reutilizarse en otras aplicaciones.

### `Cifrar`

```csharp
string textoCifrado = DpapiHelper.Cifrar(
    textoPlano: "mi-secreto",
    _entropy: Encoding.UTF8.GetBytes("mi-entropia"),
    usarMaquina: false  // false = solo usuario actual (default)
);
```

### `Descifrar`

```csharp
string textoOriginal = DpapiHelper.Descifrar(
    textoCifrado: textoCifrado,
    _entropy: Encoding.UTF8.GetBytes("mi-entropia"),
    usarMaquina: false
);
```

### `DescifrarSeguro`

Versión que captura todas las excepciones y retorna `string.Empty` en caso de error:

```csharp
string resultado = DpapiHelper.DescifrarSeguro(textoCifrado, entropy);
// Nunca lanza excepción — ideal para flujos no críticos
```

---

## ⚠️ Consideraciones de Seguridad

- Los datos cifrados con `DataProtectionScope.CurrentUser` **no pueden ser descifrados** por otro usuario, ni siquiera un administrador.
- Los datos cifrados con `DataProtectionScope.LocalMachine` solo son válidos **en el mismo equipo**.
- El cifrado **no es portátil entre máquinas**: un texto cifrado en una PC no puede descifrarse en otra.
- La entropía actúa como una contraseña adicional. Si se pierde, los datos cifrados **no pueden recuperarse**.

---

## 📄 Licencia

Este proyecto está bajo la licencia incluida en el archivo [LICENSE](LICENSE).
