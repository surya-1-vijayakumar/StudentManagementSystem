
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';

import {
  StudentService,
  Student,
  StudentRequest
} from '../../services/student';

@Component({
  selector: 'app-students',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './students.html',
  styleUrl: './students.css'
})
export class Students implements OnInit {

  private studentService = inject(StudentService);

  students: Student[] = [];

  searchText = '';

  isLoading = false;
  isEditing = false;
  errorMessage = '';
  successMessage = '';

  editingId: number | null = null;

  studentForm: StudentRequest = this.emptyForm();

  ngOnInit(): void {
    this.loadStudents();
  }

  emptyForm(): StudentRequest {
    return {
      name: '',
      email: '',
      department: '',
      age: 17
    };
  }

  // Load all students
  loadStudents(): void {
    this.isLoading = true;
    this.errorMessage = '';

    this.studentService.getStudents(this.searchText).subscribe({
      next: (data) => {
        this.students = data;
        this.isLoading = false;
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'Unable to load students.';
        this.isLoading = false;
      }
    });
  }

  // Search students
  searchStudents(): void {
    this.loadStudents();
  }

  // Add or update student
  saveStudent(): void {
    this.errorMessage = '';
    this.successMessage = '';

    if (
      !this.studentForm.name.trim() ||
      !this.studentForm.email.trim() ||
      !this.studentForm.department.trim()
    ) {
      this.errorMessage = 'Please fill in all fields.';
      return;
    }

    const request: StudentRequest = {
      name: this.studentForm.name.trim(),
      email: this.studentForm.email.trim(),
      department: this.studentForm.department.trim(),
      age: Number(this.studentForm.age)
    };

    if (this.isEditing && this.editingId !== null) {
      this.studentService
        .updateStudent(this.editingId, request)
        .subscribe({
          next: () => {
            this.successMessage = 'Student updated successfully.';
            this.cancelEdit();
            this.loadStudents();
          },
          error: (error) => {
            this.errorMessage =
              error.error?.message || 'Unable to update student.';
          }
        });
    } else {
      this.studentService.addStudent(request).subscribe({
        next: () => {
          this.successMessage = 'Student added successfully.';
          this.studentForm = this.emptyForm();
          this.loadStudents();
        },
        error: (error) => {
          this.errorMessage =
            error.error?.message || 'Unable to add student.';
        }
      });
    }
  }

  // Select student for editing
  editStudent(student: Student): void {
    this.isEditing = true;
    this.editingId = student.id;

    this.studentForm = {
      name: student.name,
      email: student.email,
      department: student.department,
      age: student.age
    };

    this.errorMessage = '';
    this.successMessage = '';
  }

  // Cancel editing
  cancelEdit(): void {
    this.isEditing = false;
    this.editingId = null;
    this.studentForm = this.emptyForm();
  }

  // Delete student
  deleteStudent(id: number): void {
    const confirmed = window.confirm(
      'Are you sure you want to delete this student?'
    );

    if (!confirmed) {
      return;
    }

    this.studentService.deleteStudent(id).subscribe({
      next: () => {
        this.successMessage = 'Student deleted successfully.';
        this.loadStudents();
      },
      error: (error) => {
        this.errorMessage =
          error.error?.message || 'Unable to delete student.';
      }
    });
  }
}
