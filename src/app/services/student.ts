
import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

export interface Student {
  id: number;
  name: string;
  email: string;
  department: string;
  age: number;
}

export interface StudentRequest {
  name: string;
  email: string;
  department: string;
  age: number;
}

@Injectable({
  providedIn: 'root'
})
export class StudentService {
  private http = inject(HttpClient);

  private apiUrl = 'https://localhost:7012/api/Students';

  // Get all students
  getStudents(search?: string): Observable<Student[]> {
    const url = search
      ? `${this.apiUrl}?search=${encodeURIComponent(search)}`
      : this.apiUrl;

    return this.http.get<Student[]>(url);
  }

  // Get student by ID
  getStudent(id: number): Observable<Student> {
    return this.http.get<Student>(`${this.apiUrl}/${id}`);
  }

  // Add student
  addStudent(data: StudentRequest): Observable<Student> {
    return this.http.post<Student>(this.apiUrl, data);
  }

  // Update student
  updateStudent(
    id: number,
    data: StudentRequest
  ): Observable<Student> {
    return this.http.put<Student>(
      `${this.apiUrl}/${id}`,
      data
    );
  }

  // Delete student
  deleteStudent(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
