# iparking
Repositorio de Aplicacion Mobile para Sistema iParking  
**Programas y Versiones para Despliegue**

- Visual Studio Comunity 2015
- Android SDK V19 y V23
- Xamarin Android Player con:  
	- Vm Nexus 5 (KitKat) Android 4.4.2 y API 19 en 1080 x 1920
  
## Configuracion Visual Studio 2015  

- Tools > Options > Xamarin > Android Settings > Preserve Aplication Data/Cache on device between deploys
- En Properties > Android Options > Advanced > Advanced Android Building Settings > Cambiar el Java Heap Size a 1G

## Release de APK (Para Testing y Demos)

- 1. Cambiar el Modo de uso del VS a Release
- 2. Limpiar la Solucion
- 3. Compilar (Build) > Exportar a Paquete Android (*.apk)
- 4. En la carpeta de la solucion: bin > Release 
- 5. Utilizar el archivo que sea nombre-Signed.apk

## Directorios

- Assets: Se incluyen archivos como fonts, musica o video necesarios para la aplicacion. De esta
forma Android entiende que son "Extras" y te permite accederlos de manera literal (sin interpretarlos)
- Resources/drawable: Todo lo que es necesario para el estilo de las vistas (Imgenes, archivos xml con estilos, etc...)
- Resources/layout: El diseño de las vistas de la aplicacion (las pantallas)
- Resources/values: Archivos de Estilo, cadenas, etc...

## Componentes

**Importante:** Cada vez que se instala un componente, es una buen practica, cerrar el Visual Studio y volverlo a abrir  
Y luego de haber instalado el componente, actualizar la referencia desde el NuGet. Y reiniciar Visual Studio...  
Una vez reiniciado, limpiar y recompilar

- **Android Support Library v7 AppCompat:** Necesaria para la Splash Screen
- **Google Play Services - Maps:** Necesaria para el uso de Google Maps. Una vez modificado el Heap Size, limpiar 
la solucion y volver a compilar (descarga librerias de Java)

