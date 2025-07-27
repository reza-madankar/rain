# Rain Data Management Frontend

A React application built with Ant Design for managing rain data records. This application provides a user-friendly interface to view, filter, and add rain data records.

## Features

- **User ID Management**: Enter your user ID to access your rain data
- **Data Table**: View rain data with pagination and sorting
- **Filtering**: Filter data by rain status (All, Rain, No Rain)
- **Add New Records**: Modal form to add new rain records
- **Real-time Updates**: Automatic refresh after adding new records
- **Error Handling**: Comprehensive error handling and validation
- **Responsive Design**: Modern UI with Ant Design components

## Technologies Used

- **React 19**: Latest React version with hooks
- **Ant Design 5**: UI component library
- **React Query (@tanstack/react-query)**: Data fetching and caching
- **Axios**: HTTP client for API calls
- **useMemo**: Performance optimization for data processing

## Getting Started

### Prerequisites

- Node.js (v14 or higher)
- npm or yarn
- Backend API running on `https://localhost:7194`

### Installation

1. Navigate to the frontend directory:
   ```bash
   cd Frontend
   ```

2. Install dependencies:
   ```bash
   npm install
   ```

3. Start the development server:
   ```bash
   npm start
   ```

4. Open [http://localhost:3000](http://localhost:3000) in your browser

## API Integration

### Endpoints Used

1. **GET** `/api/v1/Rain?page={page}&pageSize={pageSize}&isRain={isRain}`
   - Headers: `x-userid: {userId}`
   - Response: Paginated rain data with metadata

2. **POST** `/api/v1/Rain`
   - Headers: `x-userid: {userId}`, `Content-Type: application/json`
   - Body: `{ "rain": boolean }`
   - Response: Success/error message

### Features Implementation

#### useQuery Hook
- Fetches rain data with automatic caching
- Handles loading states and errors
- Refetches data when dependencies change
- Stale time set to 30 seconds

#### useMemo Hook
- Optimizes table data processing
- Prevents unnecessary re-renders
- Formats timestamps and rain status

#### Pagination
- Server-side pagination
- Configurable page size
- Quick jumper for navigation
- Total record count display

#### Filtering
- Filter by rain status (All/True/False)
- Resets to first page when filter changes
- URL parameters for API calls

#### Modal Form
- Form validation with Ant Design
- Switch component for rain status
- Error display from backend responses
- Success/error message handling

## Component Structure

```
src/
├── components/
│   ├── RainApp.js          # Main application component
│   ├── RainTable.js        # Data table with pagination
│   └── AddRainModal.js     # Modal for adding records
├── App.js                  # App wrapper with providers
├── App.css                 # Custom styles
└── index.css              # Global styles and Ant Design import
```

## Usage

1. **Enter User ID**: Type your user ID in the input field
2. **View Data**: The table will automatically load your rain data
3. **Filter Data**: Use the dropdown to filter by rain status
4. **Navigate**: Use pagination controls to browse through pages
5. **Add Record**: Click "Add New Record" button to open the modal
6. **Submit**: Toggle the switch and click "Add Record" to save

## Error Handling

- **Network Errors**: Displayed as toast messages
- **Validation Errors**: Form-level validation with Ant Design
- **Backend Errors**: Parsed and displayed in modal
- **Loading States**: Spinner indicators during API calls

## Styling

- Modern design with Ant Design components
- Responsive layout
- Custom CSS for enhanced visual appeal
- Consistent color scheme and spacing

## Development

### Available Scripts

- `npm start`: Start development server
- `npm build`: Build for production
- `npm test`: Run tests
- `npm eject`: Eject from Create React App

### Key Dependencies

- `antd`: UI component library
- `@tanstack/react-query`: Data fetching and caching
- `axios`: HTTP client
- `react`: Core React library

## Backend Requirements

Ensure your backend API is running and accessible at `https://localhost:7194` with the following endpoints:

- `GET /api/v1/Rain` - List rain data with pagination
- `POST /api/v1/Rain` - Add new rain record

Both endpoints require the `x-userid` header for authentication.
