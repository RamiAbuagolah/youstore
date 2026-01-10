import { Component } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatListModule } from '@angular/material/list';
import { MatCardModule } from '@angular/material/card';

@Component({
  selector: 'youstore-admin-root',
  standalone: true,
  imports: [MatToolbarModule, MatListModule, MatCardModule],
  template: `
    <mat-toolbar color="primary">
      <span>YouStore Admin</span>
    </mat-toolbar>

    <div class="container">
      <mat-card>
        <mat-card-title>Tenant overview</mat-card-title>
        <mat-list>
          <mat-list-item>Active merchants: 12</mat-list-item>
          <mat-list-item>Pending stores: 3</mat-list-item>
        </mat-list>
      </mat-card>

      <mat-card>
        <mat-card-title>System insights</mat-card-title>
        <p>Monitor health checks, service bus processors, and global catalogs.</p>
      </mat-card>
    </div>
  `,
  styles: [
    `
      :host {
        display: block;
        min-height: 100vh;
        background: #eef2ff;
      }

      .container {
        padding: 24px;
        display: grid;
        gap: 16px;
      }
    `
  ]
})
export class AppComponent {}
