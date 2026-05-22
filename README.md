# AlgebraOfSignatures

Библиотека для представления экстремальных k-однородных гиперграфов в виде сигнатур и выполнения над ними операций объединения, пересечения и сложения разными способами.

---

## Установка

```bash
dotnet add package AlgebraOfSignatures.Core
```

Или через Package Manager Console в Visual Studio:

```powershell
Install-Package AlgebraOfSignatures.Core
```

---

## Возможности

- **Вычисление сигнатуры** по матрице смежности гиперграфа. Сигнатура хранится в массиве произвольного ранга, соответствующего степени однородности.
- **Создание объекта гиперграфа** на основе вычисленной сигнатуры.
- **Обратное преобразование**: восстановление матрицы смежности или вектора степеней вершин из сигнатуры. Результаты кешируются и обновляются автоматически при изменении значений сигнатуры.
- **Алгебраические операции** над сигнатурами:
    - Пересечение (`&`)
    - Объединение (`|`)
    - Сложение с константой — вертикальное и горизонтальное (`+`)
    - Сложение двух сигнатур — вертикальное и горизонтальное (`+`)

---

## Использование

### Создание гиперграфа из матрицы смежности

```csharp
using AlgebraOfSignatures.Core;
using AlgebraOfSignatures.Core.Base;

// Матрица смежности в виде булева массива ранга uniformityDegree и размера vertexCount
var adjacencyArray = new bool[4, 4, 4]; // 3-однородный граф на 4 вершинах
// ... заполнение матрицы ...

var adjacencyMatrix = new Matrix<bool>(adjacencyArray);
var graph = UniformHyperGraph.FromAdjacencyMatrix(adjacencyMatrix);
```

### Создание гиперграфа из вектора степеней вершин

```csharp
var degreeArray = new int[4, 4]; // 2-однородный граф (обычный граф) на 4 вершинах
// ... заполнение вектора ...

var degreeVector = new Matrix<int>(degreeArray);
var graph = UniformHyperGraph.FromVertexDegreeVector(degreeVector);
```

### Создание гиперграфа напрямую из значения сигнатуры

```csharp
// Из объекта Signature
var signature = Signature.Empty(vertexCount: 5, uniformityDegree: 3);
var graph = UniformHyperGraph.FromSignature(signature, vertexCount: 5, uniformityDegree: 3);

// Из матрицы long-значений
var signatureMatrix = new Matrix<long>(new long[3, 2] { { 7, 3 }, { 5, 1 }, { 3, 0 } });
var graph2 = UniformHyperGraph.FromSignature(signatureMatrix, vertexCount: 5, uniformityDegree: 3);
```

### Пустой гиперграф

```csharp
var empty = UniformHyperGraph.Empty(vertexCount: 4, uniformityDegree: 2);
```

### Сохранение и загрузка из файла

```csharp
graph.SaveToFile("graph.txt");

var loaded = UniformHyperGraph.FromFile("graph.txt");
```

### Получение матрицы смежности и вектора степеней вершин

```csharp
Matrix<bool> adjacency = graph.AdjacencyMatrix;     // кешируется до изменения сигнатуры
Matrix<int>  degrees   = graph.VertexDegreeVector;  // кешируется до изменения сигнатуры
```

---

## Операции над гиперграфами

Операции применяются к объектам `UniformHyperGraph` или напрямую к `Signature`. Все операции с двумя операндами требуют совпадения `VertexCount` и `UniformityDegree`.

### Пересечение

```csharp
var result = graph1 & graph2;

// Или явным вызовом:
var result = UniformHyperGraph.Intersect(graph1, graph2);
```

### Объединение

```csharp
var result = graph1 | graph2;

// Или явным вызовом:
var result = UniformHyperGraph.Union(graph1, graph2);
```

### Сложение двух гиперграфов

```csharp
// Вертикальное сложение (по умолчанию для оператора +)
var result = graph1 + graph2;

// Горизонтальное сложение
var result = UniformHyperGraph.Add(graph1, graph2, Signature.AddType.Horizontal);
```

### Сложение с константой

```csharp
// Вертикальное (по умолчанию для оператора +)
var result = graph + 3L;

// Горизонтальное
var result = UniformHyperGraph.Add(graph, 3L, Signature.AddType.Horizontal);
```

---

## Операции напрямую над объектами класса Signature

Все операции доступны и на уровне класса `Signature`:

```csharp
var sig1 = Signature.Empty(vertexCount: 4, uniformityDegree: 2);
var sig2 = Signature.Empty(vertexCount: 4, uniformityDegree: 2);

// Установка значения
sig1.SetValue(5L, 0);

// Пересечение и объединение
var intersection = sig1 & sig2;
var union        = sig1 | sig2;

// Сложение
var sum = sig1 + sig2;
var shifted = sig1 + 2L;

// Получение значения
long val = sig1.GetValue(0);
```

---

## Матрица `Matrix<T>`

Вспомогательный класс для хранения многомерных массивов произвольного ранга. Используется как внутреннее представление матриц смежности, векторов степеней и сигнатур.

```csharp
// Из существующего массива
var matrix = new Matrix<long>(new long[3, 3]);

// Массив заданного размера и ранга, полностью заполненный нулевыми значениями 
var matrix = new Matrix<long>(size: 4, rank: 2);

// Чтение и запись
long value = matrix.GetValue(1, 2);
matrix.SetValue(42L, 1, 2);

// Событие при изменении значения
matrix.OnSetValue += (indices, value) =>
{
    Console.WriteLine($"Изменено значение по [{string.Join(", ", indices)}] = {value}");
};
```

---

## Сравнение и равенство

`Signature` и `UniformHyperGraph` реализуют `IEquatable<T>` и `IComparable<T>`, а также поддерживают операторы сравнения:

```csharp
bool eq  = graph1 == graph2;
bool neq = graph1 != graph2;
bool lt  = graph1 < graph2;
bool gt  = graph1 > graph2;
```