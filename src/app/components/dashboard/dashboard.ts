
import { Component, OnInit, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css',
})
export class Dashboard implements OnInit {
  private auth = inject(Auth);

  userName = 'User';
  userEmail = '';
  userRole = 'Student';

  ngOnInit(): void {
    const storedUser = localStorage.getItem('user');

    if (storedUser) {
      const user = JSON.parse(storedUser);
      this.userName = user.name || 'User';
      this.userEmail = user.email || '';
      this.userRole = user.role || 'Student';
    }
  }

  logout(): void {
    this.auth.logout();
  }
}
